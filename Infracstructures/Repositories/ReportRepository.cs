using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Report?> GetByIdAsync(Guid? id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status != ReportStatus.Inactive.ToString());
    }

    public override async Task<List<Report>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != ReportStatus.Inactive.ToString()).ToListAsync();
    }

    public async Task<List<Report>> GetAllReportPendingAsync()
    {
        return await DbSet.Where(x => x.Status == ReportStatus.Pending.ToString()).ToListAsync();
    }
}