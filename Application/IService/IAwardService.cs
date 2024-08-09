using Application.BaseModels;
using Application.SendModels.Award;
using Application.ViewModels.AwardViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IAwardService
{
    Task<bool> AddAward(CreateAwardSendModel addCreateAwardViewModel);
    Task<(List<AwardViewModel>, int)> GetListAward(ListModels listAwardModel);
    Task<bool> DeleteAward(Guid awardId);
    Task<bool> UpdateAward(UpdateAwardRequest updateAward);
    Task<AwardViewModel> GetAwardById(Guid awardId);

    Task<List<ListAwardViewModels>?> GetAwardsByContestId(Guid contestId);

    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateAwardRequest(CreateAwardSendModel createAward);
    Task<ValidationResult> ValidateTopicUpdateRequest(UpdateAwardRequest awardUpdate);
}