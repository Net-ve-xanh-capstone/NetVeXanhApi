using Application.ViewModels.PaintingViewModels;

namespace Application.ViewModels.ScheduleViewModels;

public class AwardScheduleResponse
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public Guid? AwardId { get; set; }
    public Guid? ScheduleId { get; set; }
    public string? Rank { get; set; }
    public string? Status { get; set; }
    public List<PaintingResponse> PaintingViewModelsList { get; set; }
}