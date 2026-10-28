using System.ComponentModel.DataAnnotations;
using CollegeEventHub.Models.Enums;

namespace CollegeEventHub.Models;

public class Registration
{
    public int Id { get; set; }
    [Required]
    public string StudentId { get; set; } = "";
    public ApplicationUser Student { get; set; } = null!;
    public int EventId { get; set; }
    public CollegeEvent Event { get; set; } = null!;
    public RegistrationStatus Status { get; set; } = RegistrationStatus.PendingPayment;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public ICollection<RegistrationResponse> Responses { get; set; } = new List<RegistrationResponse>();
    public ICollection<DummyPayment> Payments { get; set; } = new List<DummyPayment>();
}
