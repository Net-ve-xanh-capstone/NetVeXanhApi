namespace Application.ViewModels.ScheduleViewModels;

public class ScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid? RoundId { get; set; }
    public string? Description { get; set; }
    public string? Round { get; set; }
    public string? Year { get; set; }
    public Guid? ExaminerId { get; set; }
    public string? ExaminerName { get; set; }
    public string? Status { get; set; }
    public DateTime EndDate { get; set; }
}