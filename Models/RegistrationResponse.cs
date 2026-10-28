using System.ComponentModel.DataAnnotations;

namespace CollegeEventHub.Models;

public class RegistrationResponse
{
    public int Id { get; set; }
    public int RegistrationId { get; set; }
    public Registration Registration { get; set; } = null!;
    public int RegistrationFormFieldId { get; set; }
    public RegistrationFormField FormField { get; set; } = null!;
    [StringLength(4000)]
    public string? Answer { get; set; }
}
