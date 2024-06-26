using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class PaintingRepository : GenericRepository<Painting>, IPaintingRepository
{
    public PaintingRepository(AppDbContext context) : base(context)
    {
    }

    public virtual async Task<Painting?> GetByCodeAsync(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code == code);
    }

    public virtual async Task<List<Painting>> List20WiningPaintingAsync()
    {
        return await DbSet.Where(x => x.AwardId != null).OrderByDescending(x => x.UpdatedTime).Include(x => x.Award)
            .Take(20).ToListAsync();
    }

    public async Task<List<Painting>> ListPaintingForPreliminaryRound(Guid id)
    {
        return await DbSet.Where(x => x.RoundId == id && x.Status == PaintingStatus.Accepted.ToString()).OrderByDescending(x => x.UpdatedTime).ToListAsync();
    }
    
    public async Task<List<Painting>> ListPaintingForFinalRound(Guid id)
    {
        return await DbSet.Where(x => x.RoundId == id && x.Status == PaintingStatus.FinalRound.ToString()).OrderByDescending(x => x.UpdatedTime).ToListAsync();
    }

    /*public async Task<List<Acc>> ListCompetitorPassRound(Guid id)
    {
        return await DbSet.Include(p => p.Competitor).Where(p => p.Status == PaintingStatus.Pass.ToString() && p.RoundId == id).Select(p => p.Competitor).ToListAsync();
    }*/
}