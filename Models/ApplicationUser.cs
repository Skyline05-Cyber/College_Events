using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CollegeEventHub.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(120)]
    public string? FullName { get; set; }

    [StringLength(160)]
    public string? CollegeName { get; set; }

    [StringLength(100)]
    public string? Department { get; set; }
}
