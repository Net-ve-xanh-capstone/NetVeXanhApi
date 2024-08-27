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
    Task<bool> IsExistNameAsync(string name);

    Task<Contest?> GetContestByIdForRoundTopic(Guid id);

    Task<List<Contest>> GetContestByStatus(string contestStatus);
    Task<List<Contest>> EndContest();
    Task<List<Contest>> StartContest();
    Task<List<AccountAwardResponse>> GetAccountsByMostRecentContestAsync();
    Task<List<Contest>> GetContestRewardByListContestId(List<Guid> contestIdList);

    Task<Contest?> GetContestThisYear();


    #region Check

    Task<bool> CheckContestDuplicate(DateTime startTime, DateTime endTime);

    #endregion

    Task<List<NumberPaintingResponse>> GetNumberOfPaintingsByContestAsync();
    Task<List<ContestAwardQuantityResponse>> GetAwardQuantity();


    Task<List<Painting>?> GetPaintingHasPriceOfContest(Guid contestId);
}