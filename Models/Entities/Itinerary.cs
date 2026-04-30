using System.ComponentModel.DataAnnotations;

namespace WebsiteTour.Models.Entities;

public class Itinerary
{
    public int Id { get; set; }
    
    [Required]
    public int TourId { get; set; }
    
    [Required]
    public int DayNumber { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;

    public Tour? Tour { get; set; }
}
