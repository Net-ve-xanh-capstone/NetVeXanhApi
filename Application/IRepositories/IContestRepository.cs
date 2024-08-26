using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using Domain.Models;

namespace Application.IRepositories;

public interface IContestRepository : IGenericRepository<Contest>
{
    Task<List<string>> GetListEducationalLevelName(Guid contestId);
    Task<List<string>> GetListRoundName(Guid contestId);
    Task<Contest?> GetByIdForScheduleAsync(Guid? id);


    Task<Contest?> GetAllContestInformationAsync(Guid contestId);
    Task<List<ContestNameYearResponse>> Get5RecentYearAsync();
    Task<(DateTime StartTime, DateTime EndTime)?> GetStartEndTimeByContestId(Guid contestId);

    Task<Contest?> GetNearestContestInformationAsync();

    Task<List<Guid>> Get3NearestContestId();

    Task<Contest?> GetContestByIdForRoundTopic(Guid id);

    Task<List<Contest>> GetContestByStatus(string contestStatus);
    public Task<List<Contest>> EndContest();
    public Task<List<Contest>> StartContest();
    Task<List<AccountAwardResponse>> GetAccountsByMostRecentContestAsync();
    Task<List<Contest>> GetContestRewardByListContestId(List<Guid> contestIdList);

    Task<Contest?> GetContestThisYear();


    #region Check

    Task<bool> CheckContestDuplicate(DateTime startTime, DateTime endTime);

    #endregion

    public Task<List<NumberPaintingResponse>> GetNumberOfPaintingsByContestAsync();
    public Task<List<ContestAwardQuantityResponse>> GetAwardQuantity();


    public Task<List<Painting>?> GetPaintingHasPriceOfContest(Guid contestId);
}