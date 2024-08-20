using Application.BaseModels;
using Application.SendModels.Award;
using Application.ViewModels.AwardViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IAwardService
{
    Task<bool> AddAward(CreateAwardRequest addCreateAwardViewModel);
    Task<(List<AwardViewResponse>, int)> GetListAward(ListModels listAwardModel);
    Task<bool> DeleteAward(Guid awardId);
    Task<bool> UpdateAward(UpdateAwardRequest updateAward);
    Task<AwardViewResponse> GetAwardById(Guid awardId);

    Task<List<AwardViewResponse>?> GetAwardsByRoundId(Guid contestId);

    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateAwardRequest(CreateAwardRequest createAward);
    Task<ValidationResult> ValidateTopicUpdateRequest(UpdateAwardRequest awardUpdate);
}