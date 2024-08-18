namespace Application.SendModels.Schedule;

public class ScheduleForPreliminarySendModel
{
    public string? Description { get; set; }
    public Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public List<Guid> ListExaminer { get; set; }
    public int JudgedCount { get; set; }
    public Guid CurrentUserId { get; set; }
    public List<PrizeWithCountViewModel> Awards { get; set; }
}