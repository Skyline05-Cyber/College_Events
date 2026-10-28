using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CollegeEventHub.Models.Enums;

namespace CollegeEventHub.Models;

public class DummyPayment
{
    public int Id { get; set; }
    public int RegistrationId { get; set; }
    public Registration Registration { get; set; } = null!;
    [Required, StringLength(50)]
    public string TransactionReference { get; set; } = Guid.NewGuid().ToString("N");
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    public DummyPaymentStatus Status { get; set; } = DummyPaymentStatus.Pending;
    [StringLength(30)]
    public string? PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
