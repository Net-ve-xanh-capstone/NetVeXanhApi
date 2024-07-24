using Application.IRepositories;
using Application.SendModels.Painting;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class PaintingRepository : GenericRepository<Painting>, IPaintingRepository
{
    public PaintingRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Painting>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != PaintingStatus.Delete.ToString())
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .Include(x => x.Account)
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Round)
            .ThenInclude(x => x.EducationalLevel)
            .ThenInclude(x => x.Contest)
            .ToListAsync();
    }

    public virtual async Task<Painting?> GetByCodeAsync(string code)
    {
        return await DbSet.Where(x => x.Status != PaintingStatus.Delete.ToString())
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public override async Task<Painting?> GetByIdAsync(Guid id)
    {
        return await DbSet.Where(x => x.Status != PaintingStatus.Delete.ToString())
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Round)
            .ThenInclude(x => x.EducationalLevel)
            .ThenInclude(x => x.Contest)
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<List<Painting>> List16WiningPaintingAsync()
    {
        return await DbSet.Where(x => x.AwardId != null && x.Status != PaintingStatus.Delete.ToString())
            .OrderByDescending(x => x.UpdatedTime)
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .Include(x => x.Account)
            .Take(16).ToListAsync();
    }

    public async Task<List<Account>> ListCompetitorPassByRound(Guid roundId)
    {
        return await DbSet.Include(p => p.Account)
            .Where(p => p.Status == PaintingStatus.Pass.ToString() && p.RoundTopicId == roundId)
            .Select(p => p.Account).ToListAsync();
    }

    public async Task<List<Painting>> ListByAccountIdAsync(Guid accountId)
    {
        return await DbSet.Where(x => x.AccountId == accountId && x.Status != PaintingStatus.Delete.ToString())
            .Include(x => x.Account)
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .ToListAsync();
    }

    public async Task<List<Guid>> ListAccountIdByListAwardId(List<Guid> listAwardId)
    {
        return await DbSet
            .Where(x => listAwardId.Contains((Guid)x.AwardId))
            .Select(x => x.AccountId)
            .ToListAsync();
    }

    public async Task<int> CreateNewNumberOfPaintingCode(Guid? roundId)
    {
        var paintings = await DbSet.Include(p => p.RoundTopic)
            .ThenInclude(r => r.Round)
            .Where(p => p.RoundTopic.Round.Id == roundId)
            .ToListAsync();

        var maxNumber = paintings
            .Select(p => p.Code.Substring(Math.Max(0, p.Code.Length - 5)))
            .Where(code => int.TryParse(code, out _))
            .Select(code => int.Parse(code))
            .DefaultIfEmpty(0)
            .Max();

        return maxNumber + 1;
    }

    public async Task<List<Painting>> FilterPaintingAsync(FilterPaintingRequest filterPainting)
    {
        var listPainting = DbSet
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Round)
            .ThenInclude(x => x.EducationalLevel)
            .ThenInclude(x => x.Contest)
            .Include(x => x.RoundTopic)
            .ThenInclude(x => x.Topic)
            .Include(x => x.Account)
            .AsQueryable();
        if (!string.IsNullOrEmpty(filterPainting.Code))
            listPainting = listPainting.Where(p => p.Code.Contains(filterPainting.Code));
        if (!string.IsNullOrEmpty(filterPainting.TopicName))
            listPainting = listPainting.Where(p => p.RoundTopic.Topic.Name.Contains(filterPainting.TopicName));
        if (filterPainting.StartDate != null && filterPainting.EndDate != null)
            listPainting = listPainting.Where(p =>
                p.UpdatedTime >= filterPainting.StartDate && p.UpdatedTime <= filterPainting.EndDate);
        if (!string.IsNullOrEmpty(filterPainting.Level))
            listPainting =
                listPainting.Where(p => p.RoundTopic.Round.EducationalLevel.Level.Contains(filterPainting.Level));
        if (!string.IsNullOrEmpty(filterPainting.RoundName))
            listPainting = listPainting.Where(p => p.RoundTopic.Round.Name.Contains(filterPainting.RoundName));
        if (!string.IsNullOrEmpty(filterPainting.Status))
            listPainting = listPainting.Where(p => p.Status.Contains(filterPainting.Status));
        return await listPainting.ToListAsync();
    }

    public async Task<int> GetNumPaintingInContest(Guid contestId)
    {
        return await DbSet
            .Include(p => p.RoundTopic)
            .ThenInclude(r => r.Round)
            .ThenInclude(r => r.EducationalLevel)
            .Where(p => p.RoundTopic.Round.EducationalLevel.ContestId == contestId)
            .CountAsync();
    }


    public async Task<List<Painting>> GetAllPaintingOfRound(Guid id)
    {
        return await DbSet.Include(src => src.Account).Where(src => src.RoundTopic.RoundId.Equals(id)).ToListAsync();
    }

    public async Task<Painting> GetPaintingsByContestAndAccountAsync(Guid contestId, Guid accountId)
    {
        var paintings = await  DbSet.Include(x => x.RoundTopic)
                                    .ThenInclude(x => x.Round)
                                    .ThenInclude(x => x.EducationalLevel)
                                    .ThenInclude(x => x.Contest)
                                    .Include(x => x.RoundTopic)
                                    .ThenInclude(x => x.Topic)
                                    .Include(x => x.Account)
                                    .Where(x=>x.RoundTopic.Round.EducationalLevel.Contest.Id  == contestId && x.AccountId == accountId)
                                    .FirstOrDefaultAsync();
        return paintings;
    }
}