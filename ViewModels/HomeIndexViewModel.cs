using WebsiteTour.Models.Entities;
namespace WebsiteTour.ViewModels;

public class HomeIndexViewModel
{
    public List<Tour> RecommendedTours { get; set; } = [];
    public List<Tour> PopularTours { get; set; } = [];
    public List<DestinationRegionGroupViewModel> DestinationRegions { get; set; } = [];
    public int? SelectedRegionId { get; set; }
}

public class DestinationRegionGroupViewModel
{
    public int RegionId { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public string RegionSlug { get; set; } = string.Empty;
    public List<Destination> Destinations { get; set; } = [];
}
