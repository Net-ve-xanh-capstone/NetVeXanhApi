namespace Application.ViewModels.ResourcesViewModels;

public class ResourcesResponse
{
    public Guid? Id { get; set; }
    public string Sponsorship { get; set; }
    public Guid? SponsorId { get; set; }
    public string SponsorName { get; set; }
    public Guid? ContestId { get; set; }
    public string ContestName { get; set; }
}