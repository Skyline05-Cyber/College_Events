using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CollegeEventHub.Models.Enums;

namespace CollegeEventHub.Models;

public class CollegeEvent
{
    public int Id { get; set; }
    [Required, StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = "";
    [Required, StringLength(4000, MinimumLength = 10)]
    public string Description { get; set; } = "";
    [StringLength(300)]
    public string? Venue { get; set; }
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime RegistrationOpensAt { get; set; }
    public DateTime RegistrationClosesAt { get; set; }
    [Range(typeof(decimal), "0", "1000000"), Column(TypeName = "decimal(10,2)")]
    public decimal RegistrationFee { get; set; }
    [Range(1, 100000)]
    public int MaxParticipants { get; set; } = 100;
    public bool IsTeamEvent { get; set; }
    [Range(1, 100)]
    public int MinTeamSize { get; set; } = 1;
    [Range(1, 100)]
    public int MaxTeamSize { get; set; } = 1;
    [StringLength(2000)]
    public string? EligibilityCriteria { get; set; }
    [StringLength(4000)]
    public string? Rules { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public int CategoryId { get; set; }
    public EventCategory Category { get; set; } = null!;
    public string? OrganizerId { get; set; }
    public ApplicationUser? Organizer { get; set; }
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<RegistrationFormField> FormFields { get; set; } = new List<RegistrationFormField>();
}
