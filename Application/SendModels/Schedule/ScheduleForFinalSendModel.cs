using Domain.Models;

namespace Application.SendModels.Schedule;

public class ScheduleForFinalSendModel
{
    public string? Description { get; set; }
    public Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public List<Guid> ListExaminer { get; set; }
    public Guid CurrentUserId { get; set; }
    public int JudgeCount { get; set; }
    public List<PrizeWithCountViewModel> Awards { get; set; }
}
public class PrizeWithCountViewModel
{
    public Guid AwardId { get; set; }
    public int AwardCount { get; set; }
}