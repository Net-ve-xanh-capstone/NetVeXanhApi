namespace Application.SendModels.Painting;

public class UpdatePaintingRequest
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? SubmitTime { get; set; }
    public Guid? AwardId { get; set; }
    public Guid RoundTopicId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? ScheduleId { get; set; }
    public string? Code { get; set; }
    public Guid CurrentUserId { get; set; }
}