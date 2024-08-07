﻿using Application.IRepositories;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class ContestRepository : GenericRepository<Contest>, IContestRepository
{
    public ContestRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Contest>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != ContestStatus.Delete.ToString())
            .Include(x => x.Account).OrderBy(x => x.CreatedTime)
            .ToListAsync();
    }
    
    public async Task<Contest?> GetContestThisYear()
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Status == ContestStatus.Complete.ToString() && x.EndTime.Year == DateTime.Now.Year);
    }

    public async Task<Contest?> GetAllContestInformationAsync(Guid contestId)
    {
        var contest = await DbSet
            .Include(x => x.Resources.Where(r => r.Status != ResourcesStatus.Inactive.ToString()))
            .ThenInclude(r => r.Sponsor)
            .Include(x => x.EducationalLevel.Where(e => e.Status != EducationalLevelStatus.Delete.ToString()))
            .ThenInclude(e => e.Round.Where(r => r.Status != RoundStatus.Delete.ToString()))
            .ThenInclude(r => r.Award.Where(a => a.Status != AwardStatus.Inactive.ToString()))
            .Include(x => x.EducationalLevel.Where(e => e.Status != EducationalLevelStatus.Delete.ToString()))
            .ThenInclude(e => e.Round)
            .ThenInclude(r => r.RoundTopic)
            .ThenInclude(rt => rt.Topic)
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == contestId && x.Status != ContestStatus.Delete.ToString());
        return contest;
    }


    public async Task<List<ContestNameYearViewModel>> Get5RecentYearAsync()
    {
        var result = await DbSet
            .Select(x => new ContestNameYearViewModel
            {
                ContestId = x.Id,
                Year = x.Name.Length >= 4 ? x.Name.Substring(x.Name.Length - 4) : x.Name
            })
            .Take(5)
            .ToListAsync();

        return result;
    }

    public async Task<(DateTime StartTime, DateTime EndTime)?> GetStartEndTimeByContestId(Guid contestId)
    {
        var round = await DbSet
            .Where(c => c.Status != ContestStatus.Delete.ToString())
            .Include(c => c.EducationalLevel.Where(l => l.Status != EducationalLevelStatus.Delete.ToString()))
            .ThenInclude(l => l.Round)
            .SelectMany(c => c.EducationalLevel)
            .SelectMany(l => l.Round)
            .Select(r => new { r.StartTime, r.EndTime })
            .FirstOrDefaultAsync();

        return round != null ? (round.StartTime, round.EndTime) : (DateTime.MinValue, DateTime.MinValue);
    }

    public Task<Contest?> GetNearestContestInformationAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Guid>> Get3NearestContestId()
    {
        var result = await DbSet
            .Where(x => x.Status == ContestStatus.Complete.ToString())
            .OrderBy(x => x.CreatedTime).Select(x => x.Id).Take(3).ToListAsync();
        return result;
    }

    public async Task<Contest?> GetContestByIdForRoundTopic(Guid id)
    {
        var result = await DbSet.Include(c => c.EducationalLevel).ThenInclude(e => e.Round)
            .ThenInclude(r => r.RoundTopic)
            .ThenInclude(rt => rt.Topic).FirstOrDefaultAsync(c => c.Id.Equals(id));
        return result;
    }

    public async Task<List<Contest>> EndContest()
    {
        return await DbSet.Include(src => src.EducationalLevel)
            .Where(src => src.EndTime <= DateTime.Now && src.Status == ContestStatus.InProcess.ToString())
            .ToListAsync();
    }

    public async Task<List<Contest>> StartContest()
    {
        return await DbSet.Include(src => src.EducationalLevel).Where(src =>
            src.StartTime >= DateTime.Now && src.Status == ContestStatus.NotStarted.ToString()).ToListAsync();
    }
    public async Task<List<AccountAwardViewModel>> GetAccountsByMostRecentContestAsync()
    {
        /*var mostRecentContest = await DbSet.Where(x => x.Status == ContestStatus.Complete.ToString())
            .OrderByDescending(c => c.CreatedTime)
            .FirstOrDefaultAsync();

        if (mostRecentContest == null)
            return new List<AccountAwardViewModel>(); // Hoặc xử lý trường hợp không có cuộc thi nào.

        var accounts = await DbSet
            .Where(c => c.Id == mostRecentContest.Id)
            .SelectMany(c => c.EducationalLevel)
            .SelectMany(l => l.Award)
            .SelectMany(a => a.Painting)
            .Where(p => p.Award.Rank != RankAward.Preliminary.ToString())
            .Select(p => new AccountAwardViewModel
            {
                FullName = p.Account.FullName,
                Rank = p.Award.Rank
            })
            .Distinct() // Loại bỏ các đối tượng trùng lặp nếu cần
            .ToListAsync();

        return accounts;*/
        throw new NotImplementedException();
    }

    public async Task<List<Contest>> GetContestRewardByListContestId(List<Guid> contestIdList)
    {
        throw new NotImplementedException();
        /*var listContest = await DbSet
            .Where(x => contestIdList.Contains(x.Id))
            .Include(c => c.EducationalLevel) // Bao gồm EducationalLevels
            .ThenInclude(el => el.Award) // Bao gồm Awards cho mỗi EducationalLevel
            .ThenInclude(a => a.Painting) // Bao gồm Painting cho mỗi Award
            .ThenInclude(p => p.Account) // Bao gồm Account cho mỗi Painting
            .ToListAsync();

        var filteredContests = listContest
            .Select(c => new
            {
                Contest = c,
                EducationalLevel = c.EducationalLevel
                    .Select(el => new
                    {
                        EducationalLevel = el,
                        Awards = el.Award
                            .Where(a => a.Rank == RankAward.FirstPrize.ToString() ||
                                        a.Rank == RankAward.SecondPrize.ToString())
                            .ToList()
                    })
                    .Where(el => el.Awards.Any()) // Giữ lại các cấp học nếu có giải thưởng đã lọc
                    .ToList()
            })
            .Where(x => x.EducationalLevel
                .Any()) // Giữ lại các cuộc thi nếu có ít nhất một cấp học với giải thưởng đã lọc
            .Select(x => new Contest
            {
                Id = x.Contest.Id,
                Name = x.Contest.Name,
                EducationalLevel = x.EducationalLevel
                    .Select(el => new EducationalLevel
                    {
                        Id = el.EducationalLevel.Id,
                        Level = el.EducationalLevel.Level,
                        Award = el.Awards
                    })
                    .ToList()
            })
            .ToList();

        return filteredContests;*/
    }


    #region Check

    public async Task<bool> CheckContestDuplicate(DateTime startTime, DateTime endTime)
    {
        return await DbSet.AnyAsync(src =>
            src.Status != ContestStatus.Delete.ToString() &&
            ((startTime >= src.StartTime && startTime <= src.EndTime) ||
             (endTime >= src.StartTime && endTime <= src.EndTime) ||
             (startTime <= src.StartTime && endTime >= src.EndTime)));
    }

    #endregion
    
    
}