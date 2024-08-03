using Application.IRepositories;
using Domain.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class DistrictRepository : GenericRepository<District>, IDistrictRepository
{
    public DistrictRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<District?> GetByIdAsync(Guid id)
    {
        return await DbSet.Include(src => src.Wards).FirstAsync(src => src.Id == id);
    }
}