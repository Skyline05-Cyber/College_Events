using System.Security.Claims;
using CollegeEventHub.Data;
using CollegeEventHub.Models;
using CollegeEventHub.Models.Enums;
using CollegeEventHub.Services;
using CollegeEventHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Controllers;

[Authorize]
public class RegistrationsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly DummyPaymentService _payments;
    public RegistrationsController(ApplicationDbContext db, DummyPaymentService payments)
    {
        _db = db;
        _payments = payments;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int eventId)
    {
        var item = await _db.Events.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId && e.Status == EventStatus.Published);
        if (item is null) return NotFound();
        if (!CanRegister(item)) return BadRequest("Registration is not open.");
        var user = await _db.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return View(new RegistrationViewModel
        {
            EventId = item.Id,
            FullName = user?.FullName ?? "",
            Email = user?.Email ?? "",
            CollegeName = user?.CollegeName,
            Department = user?.Department
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegistrationViewModel model)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (studentId is null) return Challenge();

        var item = await _db.Events.FirstOrDefaultAsync(e =>
            e.Id == model.EventId && e.Status == EventStatus.Published);
        if (item is null) return NotFound();
        if (!CanRegister(item)) ModelState.AddModelError("", "Registration is not currently open.");

        if (await _db.Registrations.AnyAsync(r => r.EventId == model.EventId && r.StudentId == studentId))
            ModelState.AddModelError("", "You have already registered for this event.");

        var confirmedCount = await _db.Registrations.CountAsync(r =>
            r.EventId == model.EventId && r.Status == RegistrationStatus.Confirmed);
        if (confirmedCount >= item.MaxParticipants)
            ModelState.AddModelError("", "This event is full.");

        if (!ModelState.IsValid) return View(model);

        var user = await _db.Users.FindAsync(studentId);
        if (user is null) return Challenge();

        user.FullName = model.FullName.Trim();
        user.PhoneNumber = model.Phone.Trim();
        user.CollegeName = model.CollegeName?.Trim();
        user.Department = model.Department?.Trim();

        var registration = new Registration
        {
            StudentId = studentId,
            EventId = item.Id,
            Status = item.RegistrationFee == 0 ? RegistrationStatus.Confirmed : RegistrationStatus.PendingPayment
        };
        _db.Registrations.Add(registration);
        await _db.SaveChangesAsync();

        if (item.RegistrationFee == 0)
            return RedirectToAction(nameof(Confirmation), new { id = registration.Id });

        return RedirectToAction("Checkout", "DummyPayments", new { registrationId = registration.Id });
    }

    public async Task<IActionResult> Mine()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var registrations = await _db.Registrations.AsNoTracking()
            .Include(r => r.Event).Include(r => r.Payments)
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.RegisteredAt).ToListAsync();
        return View(registrations);
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var registration = await _db.Registrations.AsNoTracking()
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == id && r.StudentId == studentId &&
                r.Status == RegistrationStatus.Confirmed);
        return registration is null ? NotFound() : View(registration);
    }

    private static bool CanRegister(CollegeEvent item)
    {
        var now = DateTime.Now;
        return item.Status == EventStatus.Published &&
               now >= item.RegistrationOpensAt &&
               now < item.RegistrationClosesAt &&
               now < item.StartDate;
    }
}
