using Application.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class PaintingCollectionRepository : GenericRepository<PaintingCollection>, IPaintingCollectionRepository
{
    public PaintingCollectionRepository(AppDbContext context) : base(context)
    {
        
    }
    public async Task<bool> IsPaintingInCollectionAsync(Guid paintingId, Guid collectionId)
    {
        return await DbSet.AnyAsync(pc => pc.PaintingId == paintingId && pc.CollectionId == collectionId);
    }
}