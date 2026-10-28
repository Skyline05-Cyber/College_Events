using CollegeEventHub.Data;
using CollegeEventHub.Models;
using CollegeEventHub.Models.Enums;
using CollegeEventHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Controllers;

public class EventsController : Controller
{
    private readonly ApplicationDbContext _db;
    public EventsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, int? categoryId)
    {
        var query = _db.Events.AsNoTracking().Include(e => e.Category)
            .Where(e => e.Status == EventStatus.Published);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(e => e.Title.Contains(term) || e.Description.Contains(term));
        }
        if (categoryId.HasValue)
            query = query.Where(e => e.CategoryId == categoryId.Value);

        var model = new EventListViewModel
        {
            Events = await query.OrderBy(e => e.StartDate).ToListAsync(),
            Categories = await _db.EventCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(),
            Search = search,
            CategoryId = categoryId
        };
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.Events.AsNoTracking().Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id && e.Status == EventStatus.Published);
        return item is null ? NotFound() : View(item);
    }
}
