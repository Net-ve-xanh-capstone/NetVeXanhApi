namespace Application.ViewModels.AwardViewModels;

public class AwardViewResponse
{
    public Guid Id { get; set; }
    public string? Rank { get; set; }
    public int Quantity { get; set; }
    public double? Cash { get; set; }
    public string? Artifact { get; set; }
    public string? Description { get; set; }
}