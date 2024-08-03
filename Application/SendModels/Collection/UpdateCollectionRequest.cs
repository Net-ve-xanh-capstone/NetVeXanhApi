using Domain;

namespace Application.SendModels.Collection;

public class UpdateCollectionRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; } = AppConstants.DefaultImageUrl;
    public string Description { get; set; }
    public Guid CurrentUserId { get; set; }
}