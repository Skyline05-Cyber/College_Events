using System.Security.Claims;
using CollegeEventHub.Data;
using CollegeEventHub.Models.Enums;
using CollegeEventHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Controllers;

[Authorize]
public class DummyPaymentsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly DummyPaymentService _service;

    public DummyPaymentsController(ApplicationDbContext db, DummyPaymentService service)
    {
        _db = db;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int registrationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var registration = await _db.Registrations.Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == registrationId && r.StudentId == userId);
        if (registration is null) return NotFound();
        if (registration.Status == RegistrationStatus.Confirmed)
            return RedirectToAction("Confirmation", "Registrations", new { id = registration.Id });
        if (registration.Event.RegistrationFee <= 0) return BadRequest("This event does not require payment.");

        ViewBag.RegistrationId = registration.Id;
        ViewBag.EventTitle = registration.Event.Title;
        ViewBag.Amount = registration.Event.RegistrationFee;
        return View(registration);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Process(int registrationId, string outcome, string method)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var registration = await _db.Registrations.Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == registrationId && r.StudentId == userId);
        if (registration is null) return NotFound();
        if (registration.Status == RegistrationStatus.Confirmed)
            return RedirectToAction("Confirmation", "Registrations", new { id = registration.Id });
        if (registration.Event.RegistrationFee <= 0) return BadRequest();

        var allowedMethods = new[] { "UPI (Demo)", "Card (Demo)", "Net Banking (Demo)" };
        if (!allowedMethods.Contains(method)) return BadRequest("Invalid demo payment method.");
        if (!new[] { "success", "failed", "cancelled" }.Contains(outcome))
            return BadRequest("Invalid demo outcome.");

        var payment = _service.CreateAttempt(registration, registration.Event.RegistrationFee, method);
        payment.Status = outcome switch
        {
            "success" => DummyPaymentStatus.Success,
            "failed" => DummyPaymentStatus.Failed,
            _ => DummyPaymentStatus.Cancelled
        };
        payment.CompletedAt = DateTime.UtcNow;
        _db.DummyPayments.Add(payment);

        registration.Status = outcome switch
        {
            "success" => RegistrationStatus.Confirmed,
            "failed" => RegistrationStatus.PaymentFailed,
            _ => RegistrationStatus.Cancelled
        };

        await _db.SaveChangesAsync();

        if (outcome == "success")
            return RedirectToAction("Confirmation", "Registrations", new { id = registration.Id });

        return RedirectToAction(nameof(Result), new { id = payment.Id });
    }

    public async Task<IActionResult> Result(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var payment = await _db.DummyPayments.Include(p => p.Registration)
            .ThenInclude(r => r.Event)
            .FirstOrDefaultAsync(p => p.Id == id && p.Registration.StudentId == userId);
        return payment is null ? NotFound() : View(payment);
    }
}
