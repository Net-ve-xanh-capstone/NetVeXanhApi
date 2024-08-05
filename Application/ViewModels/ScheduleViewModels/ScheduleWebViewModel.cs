namespace Application.ViewModels.ScheduleViewModels;

public class ScheduleWebViewModel
{
    public Guid Id { get; set; }
    public string Level { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ScheduleViewModel>? ScheduleViewModels { get; set; }
}