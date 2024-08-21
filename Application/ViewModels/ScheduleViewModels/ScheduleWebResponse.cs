namespace Application.ViewModels.ScheduleViewModels;

public class ScheduleWebResponse
{
    public Guid Id { get; set; }
    public string Level { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ScheduleResponse>? ScheduleViewModels { get; set; }
}