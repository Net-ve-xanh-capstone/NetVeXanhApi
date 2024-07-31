using Domain.Models;

namespace Application.IRepositories;

public interface IPaintingCollectionRepository : IGenericRepository<PaintingCollection>
{
    Task<bool> IsPaintingInCollectionAsync(Guid paintingId, Guid collectionId);
}