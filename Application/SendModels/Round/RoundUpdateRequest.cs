namespace Application.SendModels.Round;

public class RoundUpdateRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public Guid CurrentUserId { get; set; }
    public string? Status { get; set; }
    public DateTime? DeadlineSubmissionDate { get; set; }
    public DateTime? ResultAnnouncementDate { get; set; }
}