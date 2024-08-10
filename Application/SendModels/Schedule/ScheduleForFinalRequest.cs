using Domain.Models;

namespace Application.SendModels.Schedule;

public class ScheduleForFinalRequest
{
    public string? Description { get; set; }
    public Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ExaminerId { get; set; }
    public int JudgedCount { get; set; }
    public int FirstPrizeCount { get; set; }
    public int SecondPrizeCount { get; set; }
    public int ThirdPrizeCount { get; set; }
    public int ConsolationPrizeCount { get; set; }
    public Guid CurrentUserId { get; set; }
}