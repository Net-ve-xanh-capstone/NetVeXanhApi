using Application.BaseModels;
using Application.SendModels.Contest;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IContestService
{
    //Task<bool> AddContest(ContestRequest addContestViewModel);

    Task<bool> CreateContest(CreateContestRequest model);
    Task<bool> DeleteContest(Guid contestId);

    Task<bool> UpdateContest(UpdateContestRequest updateContestRequest);

    Task<ContestDetailResponse?> GetContestById(Guid contestId);

    Task<List<ContestNameYearResponse>> Get5RecentYear();

    Task<List<ContestResponse?>> GetAllContest();
    Task<(List<ContestResponse?>, int)> GetAllContest_v2(ListModels listModel);
    Task<List<FilterPaintingContestResponse>> GetContestForFilterPainting();
    Task<ContestDetailResponse> GetNearestContest();
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateContestRequest(ContestRequest contest);

    Task<ValidationResult> ValidateContestUpdateRequest(UpdateContestRequest contestRequestUpdate);
    Task<List<AccountAwardResponse>> GetAccountWithAwardPainting();
    Task<ListDropDownContestResponse> GetListForDorpDown(Guid contestId);

    Task<List<NumberPaintingResponse>> QuantiyPaintingForYear();
    Task<List<ContestAwardQuantityResponse>> AwardQuantiy();

}