using Domain;

namespace Application.SendModels.Contest;

public class ContestRequest
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Content { get; set; }
    public string Logo { get; set; } = AppConstants.DefaultImageUrl;
    public Guid CurrentUserId { get; set; }
    public DateTime Round1StartTime { get; set; }
    public DateTime Round1EndTime { get; set; }
    public DateTime Round2StartTime { get; set; }
    public DateTime Round2EndTime { get; set; }
    public int Rank1 { get; set; }
    public int Rank2 { get; set; }
    public int Rank3 { get; set; }
    public int Rank4 { get; set; }
    public int PassRound1 { get; set; }
}