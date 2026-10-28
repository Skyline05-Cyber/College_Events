using CollegeEventHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Data;

public static class DbSeeder
{
    public static async Task SeedCategoriesAsync(ApplicationDbContext db)
    {
        // Categories are seeded via EF Core HasData and migrations.
        await Task.CompletedTask;
    }

    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Organizer", "Student" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
