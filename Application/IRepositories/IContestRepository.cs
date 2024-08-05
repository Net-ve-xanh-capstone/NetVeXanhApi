using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using Domain.Models;

namespace Application.IRepositories;

public interface IContestRepository : IGenericRepository<Contest>
{
    Task<Contest?> GetAllContestInformationAsync(Guid contestId);
    Task<List<ContestNameYearViewModel>> Get5RecentYearAsync();
    Task<(DateTime StartTime, DateTime EndTime)?> GetStartEndTimeByContestId(Guid contestId);

    Task<Contest?> GetNearestContestInformationAsync();

    Task<List<Guid>> Get3NearestContestId();

    Task<Contest?> GetContestByIdForRoundTopic(Guid id);

    Task<bool> CheckContestExist(DateTime startTime);

    public Task<List<Contest>> EndContest();
    public Task<List<Contest>> StartContest();
    Task<List<Guid>> GetCollectionsWithStaffAccountsAsync();
    Task<List<AccountAwardViewModel>> GetAccountsByMostRecentContestAsync();
    Task<List<Contest>> GetContestRewardByListContestId(List<Guid> contestIdList);
}