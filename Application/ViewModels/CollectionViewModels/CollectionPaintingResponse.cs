namespace Application.ViewModels.CollectionViewModels;

public class CollectionPaintingResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public string Status { get; set; }
    public DateTime UpdatedTime { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public AccountInPainting Account { get; set; }
    public List<PaintingCollectionInCollectionViewModel> PaintingCollection { get; set; }
}

public class PaintingCollectionInCollectionViewModel
{
    public PaintingInCollectionViewModel Painting { get; set; }
}

public class PaintingInCollectionViewModel
{
    public Guid Id { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
}

public class AccountInPainting
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
}