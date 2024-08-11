namespace Application.SendModels.Schedule;

public class ScheduleRequest
{
    public string? Description { get; set; }
    public Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ExaminerId { get; set; }
    public int JudgedCount { get; set; }
    public int PassedCount { get; set; }
    public Guid CurrentUserId { get; set; }
}