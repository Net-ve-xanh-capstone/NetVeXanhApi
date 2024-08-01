using Application.IRepositories;
using Application.ViewModels.CollectionViewModels;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
{
    public CollectionRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Collection>> GetAllAsync()
    {
        var collections = await DbSet.Where(x => x.Status == CollectionStatus.Active.ToString())
        .Include(x => x.Account)
        .Include(x => x.PaintingCollection)
            .ThenInclude(pc => pc.Painting)
        .ToListAsync();

        foreach (var collection in collections)
        {
            collection.PaintingCollection = collection.PaintingCollection
                .Take(3)
                .ToList();
        }

        return collections;
    }

    public override async Task<Collection?> GetByIdAsync(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status == CollectionStatus.Active.ToString());
    }

    public virtual async Task<Collection?> GetPaintingByCollectionAsync(Guid collectionId)
    {
        return await DbSet.Where(x => x.Id == collectionId)
            .Include(x => x.PaintingCollection)
                .ThenInclude(pc => pc.Painting)
                .ThenInclude(p => p.RoundTopic)
                .ThenInclude(rt => rt.Topic)
            .Include(x => x.PaintingCollection)
                .ThenInclude(pc => pc.Painting)
                .ThenInclude(p => p.RoundTopic)
                .ThenInclude(rt => rt.Round)
                .ThenInclude(r => r.EducationalLevel)
                .ThenInclude(l => l.Contest)
            .Include(x => x.PaintingCollection)
                .ThenInclude(pc => pc.Painting)
                .ThenInclude(p => p.Account)
            .Include(x => x.PaintingCollection)
                .ThenInclude(pc => pc.Painting)
                .ThenInclude(a=>a.Award)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<List<Collection>> GetCollectionByAccountIdAsync(Guid accountId)
    {
        return await DbSet.Where(x => x.CreatedBy == accountId && x.Status == CollectionStatus.Active.ToString())
            .Include(x => x.Account).ToListAsync();
    }

    public async Task<List<Collection>> GetCollectionsWithStaffAccountsAsync()
    {
        var collections = await DbSet
            .Include(c => c.Account)
            .Where(c => c.Account.Role == "Staff")
            .Include(x => x.PaintingCollection)
            .ThenInclude(x => x.Painting)
            .ToListAsync();
        return collections;
    }

    
}