namespace Application.ViewModels.PaintingViewModels;

public class PaintingTrackingViewModel
{
    public Guid Id { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string OwnerName { get; set; }
    public History History { get; set; }
}

public class Tracking
{
    public DateTime? Time { get; set; }
    public string? Message { get; set; }
}

public class History
{
    public Tracking? Created { get; set; }
    public Tracking? Updated { get; set; }
    public Tracking? Submitted { get; set; }
    public Tracking? Reviewed { get; set; }
    public Tracking? FinalDecision { get; set; }
}