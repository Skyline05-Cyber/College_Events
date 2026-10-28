using System.ComponentModel.DataAnnotations;

namespace CollegeEventHub.Models;

public class EventCategory
{
    public int Id { get; set; }
    [Required, StringLength(80)]
    public string Name { get; set; } = "";
    [StringLength(300)]
    public string? Description { get; set; }
    public ICollection<CollegeEvent> Events { get; set; } = new List<CollegeEvent>();
}
