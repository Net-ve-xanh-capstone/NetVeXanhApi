namespace Application.ViewModels.AwardViewModels;

public class ListAwardInScheduleResponse
{
    public Guid Id { get; set; }
    public string? Rank { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
}