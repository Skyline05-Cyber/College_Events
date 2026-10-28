using CollegeEventHub.Data;
using CollegeEventHub.Models;
using CollegeEventHub.Models.Enums;
using CollegeEventHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Controllers;

[Authorize(Roles = "Admin")]
public class AdminEventsController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminEventsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var events = await _db.Events.AsNoTracking().Include(e => e.Category)
            .OrderByDescending(e => e.Id).ToListAsync();
        return View(events);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new EventFormViewModel();
        await LoadCategories(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventFormViewModel model)
    {
        await ValidateCategory(model);
        if (!ModelState.IsValid)
        {
            await LoadCategories(model);
            return View(model);
        }

        var item = new CollegeEvent();
        CopyToEntity(model, item);
        _db.Events.Add(item);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Event created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.Events.FindAsync(id);
        if (item is null) return NotFound();
        var model = new EventFormViewModel();
        CopyToViewModel(item, model);
        await LoadCategories(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventFormViewModel model)
    {
        var item = await _db.Events.FindAsync(id);
        if (item is null) return NotFound();
        await ValidateCategory(model);
        if (!ModelState.IsValid)
        {
            await LoadCategories(model);
            return View(model);
        }
        CopyToEntity(model, item);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Event updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, EventStatus status)
    {
        if (!Enum.IsDefined(typeof(EventStatus), status)) return BadRequest();
        var item = await _db.Events.FindAsync(id);
        if (item is null) return NotFound();
        item.Status = status;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Event status updated.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategories(EventFormViewModel model)
    {
        model.Categories = await _db.EventCategories.AsNoTracking().OrderBy(c => c.Name)
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
    }

    private async Task ValidateCategory(EventFormViewModel model)
    {
        if (model.CategoryId.HasValue &&
            !await _db.EventCategories.AnyAsync(c => c.Id == model.CategoryId.Value))
            ModelState.AddModelError(nameof(model.CategoryId), "Select a valid event category.");
    }

    private static void CopyToEntity(EventFormViewModel s, CollegeEvent t)
    {
        t.Title = s.Title; t.Description = s.Description; t.Venue = s.Venue; t.ImageUrl = s.ImageUrl;
        t.StartDate = s.StartDate!.Value; t.EndDate = s.EndDate!.Value;
        t.RegistrationOpensAt = s.RegistrationOpensAt!.Value;
        t.RegistrationClosesAt = s.RegistrationClosesAt!.Value;
        t.RegistrationFee = s.RegistrationFee; t.MaxParticipants = s.MaxParticipants;
        t.IsTeamEvent = s.IsTeamEvent; t.MinTeamSize = s.MinTeamSize; t.MaxTeamSize = s.MaxTeamSize;
        t.EligibilityCriteria = s.EligibilityCriteria; t.Rules = s.Rules;
        t.CategoryId = s.CategoryId!.Value; t.Status = s.Status;
    }

    private static void CopyToViewModel(CollegeEvent s, EventFormViewModel t)
    {
        t.Title = s.Title; t.Description = s.Description; t.Venue = s.Venue; t.ImageUrl = s.ImageUrl;
        t.StartDate = s.StartDate; t.EndDate = s.EndDate;
        t.RegistrationOpensAt = s.RegistrationOpensAt; t.RegistrationClosesAt = s.RegistrationClosesAt;
        t.RegistrationFee = s.RegistrationFee; t.MaxParticipants = s.MaxParticipants;
        t.IsTeamEvent = s.IsTeamEvent; t.MinTeamSize = s.MinTeamSize; t.MaxTeamSize = s.MaxTeamSize;
        t.EligibilityCriteria = s.EligibilityCriteria; t.Rules = s.Rules;
        t.CategoryId = s.CategoryId; t.Status = s.Status;
    }
}
