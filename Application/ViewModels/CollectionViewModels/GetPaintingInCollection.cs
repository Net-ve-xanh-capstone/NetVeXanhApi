namespace Application.ViewModels.CollectionViewModels;

public class GetPaintingInCollection
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<PaintingInCollection2ViewModel> Painting { get; set; }
}

public class PaintingInCollection2ViewModel
{
    public string Image { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TopicId { get; set; }
    public string TopicName { get; set; }
    public string ContestName { get; set; }
    public string Code { get; set; }
    public string OwnerName { get; set; }
    public string OwnerRole { get; set; }
    public string OwnerImage { get; set; }
    public string Rank { get; set; }
}