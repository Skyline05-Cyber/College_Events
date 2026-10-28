using System.ComponentModel.DataAnnotations;
using CollegeEventHub.Models.Enums;

namespace CollegeEventHub.Models;

public class RegistrationFormField
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public CollegeEvent Event { get; set; } = null!;
    [Required, StringLength(100)]
    public string Label { get; set; } = "";
    public FormFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? OptionsJson { get; set; }
    public int? MaxLength { get; set; }
    public ICollection<RegistrationResponse> Responses { get; set; } = new List<RegistrationResponse>();
}
