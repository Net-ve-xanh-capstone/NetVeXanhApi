using Domain.Models;

namespace Application.IRepositories;

public interface ICollectionRepository : IGenericRepository<Collection>
{
    Task<Collection?> GetPaintingByCollectionAsync(Guid collectionId);
    Task<List<Collection>> GetCollectionByAccountIdAsync(Guid accountId);
    Task<List<Collection>> GetCollectionsWithStaffAccountsAsync();
}