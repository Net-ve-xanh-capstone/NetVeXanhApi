namespace Application.SendModels.PaintingCollection;

public class PaintingCollectionRequest
{
    public List<Guid> ListPainting { get; set; }

    public Guid CollectionId { get; set; }
}