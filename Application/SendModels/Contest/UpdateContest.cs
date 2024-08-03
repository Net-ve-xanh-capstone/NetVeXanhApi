namespace Application.SendModels.Contest;

public class UpdateContest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Content { get; set; }
    public Guid CurrentUserId { get; set; }
}