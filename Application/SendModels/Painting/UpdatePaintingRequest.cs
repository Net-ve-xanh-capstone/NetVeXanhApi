namespace Application.SendModels.Painting;

public class UpdatePaintingRequest
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public Guid? RoundTopicId { get; set; }
    public Guid CurrentUserId { get; set; }
}