using Application.BaseModels;
using Application.SendModels.Round;
using Application.ViewModels.RoundViewModels;
using Application.ViewModels.TopicViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IRoundService
{
    public Task<bool> CreateRound(CreateRoundSendModel model);
    public Task<List<RoundViewModel>> GetListRoundForCompetitor();
    public Task<List<RoundViewModel>> GetListRound(ListModels listModels);
    public Task<RoundViewModel?> GetRoundById(Guid id);
    public Task<bool> UpdateRound(RoundUpdateRequest updateRound);
    Task<bool> DeleteRound(Guid id);
    Task<(List<TopicViewModel>, int)> GetTopicInRound(Guid id, ListModels listModels);
    Task<(List<RoundViewModel>, int)> GetRoundByEducationalLevelId(ListModels listLevelModel, Guid levelId);
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateRoundRequest(RoundRequest round);
    Task<ValidationResult> ValidateRoundUpdateRequest(RoundUpdateRequest roundUpdate);
}