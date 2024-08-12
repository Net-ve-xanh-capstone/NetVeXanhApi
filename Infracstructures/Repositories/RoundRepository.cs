using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class RoundRepository : GenericRepository<Round>, IRoundRepository
{
    public RoundRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Round?> GetByIdAsync(Guid? id)
    {
        return await DbSet.Include(src => src.Award).Include(src => src.Schedule)
            .Include(r => r.EducationalLevel).ThenInclude(e => e.Round)
            .FirstOrDefaultAsync(src => src.Id == id && src.Status != RoundStatus.Delete.ToString());
    }

    public override async Task<List<Round>> GetAllAsync()
    {
        return await DbSet.Include(r => r.EducationalLevel)
            .ThenInclude(c => c.Contest)
            .Where(x => x.Status != RoundStatus.Delete.ToString())
            .ToListAsync();
    }

    public Task<Round?> GetRoundDetail(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Topic>> GetTopic(Guid roundId)
    {
        return await DbSet.Where(src => src.Id == roundId && src.Status != RoundStatus.Delete.ToString())
            .SelectMany(src =>
                src.RoundTopic.Select(src => src.Topic).Where(src => src.Status == TopicStatus.Active.ToString()))
            .ToListAsync();
    }

    public async Task<List<Round>> GetRoundByLevelId(Guid levelId)
    {
        return await DbSet.Include(src => src.EducationalLevel).Include(src => src.Schedule).Where(src =>
                src.EducationalLevelId == levelId && src.Status != RoundStatus.Delete.ToString())
            .ToListAsync();
    }

    public async Task<List<Round>> GetRoundByContestId(Guid id)
    {
        var list = await DbSet.Include(src => src.EducationalLevel).ThenInclude(src => src.Contest)
            .Include(src => src.Schedule).ThenInclude(src => src.Account).Where(src =>
                src.EducationalLevel.ContestId == id && src.Status != RoundStatus.Delete.ToString())
            .ToListAsync();
        return list;
    }

    public async Task<List<Round>> GetRoundsOfThisYear()
    {
        var list = await DbSet.Include(src => src.EducationalLevel).ThenInclude(src => src.Contest)
            .Include(src => src.Schedule).Where(src =>
                src.EducationalLevel.Contest.StartTime.Year == DateTime.Today.Year &&
                src.EducationalLevel.Contest.Status != ContestStatus.Delete.ToString() &&
                src.Status == RoundStatus.InProcess.ToString()).ToListAsync();
        return list;
    }

    public virtual async Task<bool> CheckSubmitValidDate(Guid? roundId)
    {
        var temp = await DbSet.FirstOrDefaultAsync(src => src.Id == roundId);
        var check = temp.Status == RoundStatus.InProcess.ToString();
        return check;
    }


    public async Task<List<Round>> EndRound()
    {
        return await DbSet.Where(src => src.EndTime <= DateTime.Now && src.Status == RoundStatus.InProcess.ToString())
            .ToListAsync();
    }

    public async Task<List<Round>> StartRound()
    {
        return await DbSet
            .Where(src => src.StartTime >= DateTime.Now && src.Status == RoundStatus.NotStarted.ToString())
            .ToListAsync();
    }
}