using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
{
    public SponsorRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Sponsor?> GetByIdAsync(Guid? id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status == SponsorStatus.Active.ToString());
    }

    public override async Task<List<Sponsor>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status == SponsorStatus.Active.ToString()).ToListAsync();
    }
}