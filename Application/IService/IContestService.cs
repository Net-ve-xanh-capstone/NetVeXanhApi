using Application.BaseModels;
using Application.SendModels.Contest;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IContestService
{
    //Task<bool> AddContest(ContestRequest addContestViewModel);

    Task<bool> CreateContest(CreateContestSendModel model);
    Task<bool> DeleteContest(Guid contestId);

    Task<bool> UpdateContest(UpdateContest updateContest);

    Task<ContestDetailViewModel?> GetContestById(Guid contestId);

    Task<List<ContestNameYearViewModel>> Get5RecentYear();

    Task<List<ContestViewModel?>> GetAllContest();
    Task<(List<ContestViewModel?>, int)> GetAllContest_v2(ListModels listModel);
    Task<List<FilterPaintingContestViewModel>> GetContestForFilterPainting();
    Task<ContestDetailViewModel> GetNearestContest();
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateContestRequest(ContestRequest contest);

    Task<ValidationResult> ValidateContestUpdateRequest(UpdateContest contestUpdate);
    Task<List<AccountAwardViewModel>> GetAccountWithAwardPainting();
}