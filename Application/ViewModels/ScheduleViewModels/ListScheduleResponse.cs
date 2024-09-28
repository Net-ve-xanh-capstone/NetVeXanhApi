namespace Application.ViewModels.ScheduleViewModels;

public class ListScheduleResponse
{
    public Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public string RoundName { get; set; } = null!;
    public string RoundStatus { get; set; }
    public string EducationName { get; set; } = null!;
    public int TotalPainting { get; set; }
    public int PaintingNoSchedule { get; set; }
    public int PaintingWithSchedule { get; set; }
    public List<ScheduleResponse>? Schedules { get; set; }
}