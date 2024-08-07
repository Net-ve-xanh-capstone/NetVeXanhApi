using Application.SendModels.Image;

namespace Application.SendModels.Post;

public class PostUpdateRequest
{
    public Guid Id { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid CurrentUserId { get; set; }
    public List<ImageRequest>? NewImages { get; set; }
}