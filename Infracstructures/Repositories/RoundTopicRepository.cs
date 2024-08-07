using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class RoundTopicRepository : GenericRepository<RoundTopic>, IRoundTopicRepository
{
    public RoundTopicRepository(AppDbContext context) : base(context)
    {
    }

    public override Task<List<RoundTopic>> GetAllAsync()
    {
        return DbSet.Include(src => src.Round).Include(src => src.Topic).ToListAsync();
    }

    public override Task<RoundTopic?> GetByIdAsync(Guid id)
    {
        return DbSet.Include(src => src.Round).ThenInclude(src => src.EducationalLevel)
            .FirstOrDefaultAsync(src => src.Id == id);
    }

    public async Task<List<Painting>> ListPaintingForPreliminaryRound(Guid roundId)
    {
        return await DbSet.Include(tr => tr.Painting)
            .Where(tr => tr.RoundId == roundId)
            .SelectMany(tr => tr.Painting)
            .Where(p => p.Status == PaintingStatus.Accepted.ToString())
            .OrderByDescending(p => p.UpdatedTime)
            .ToListAsync();
    }

    public async Task<List<RoundTopic>> ListRoundTopicByRoundId(Guid roundId)
    {
        return await DbSet.Include(src => src.Topic).Where(src => src.RoundId == roundId).ToListAsync();
    }

    public async Task<List<Painting>> ListPaintingForFinalRound(Guid roundId)
    {
        return await DbSet
            .Where(tr => tr.RoundId == roundId)
            .Include(tr => tr.Painting)
            .SelectMany(tr => tr.Painting)
            .Where(p => p.Status == PaintingStatus.FinalRound.ToString())
            .OrderByDescending(p => p.UpdatedTime)
            .ToListAsync();
    }

    public async Task<Guid?> GetRoundTopicId(Guid roundId, Guid topicId)
    {
        return await DbSet.Where(rt => rt.RoundId == roundId && rt.TopicId == topicId)
            .Select(rt => rt.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<RoundTopic?> GetByRoundIdTopicId(Guid roundId, Guid topicId)
    {
        return await DbSet.Where(rt => rt.RoundId == roundId && rt.TopicId == topicId)
            .FirstOrDefaultAsync();
    }
}