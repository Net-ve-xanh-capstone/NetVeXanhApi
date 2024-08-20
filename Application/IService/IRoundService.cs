using Application.BaseModels;
using Application.SendModels.Round;
using Application.ViewModels.RoundViewModels;
using Application.ViewModels.TopicViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IRoundService
{
    public Task<bool> CreateRound(CreateRoundRequest model);
    public Task<List<RoundResponse>> GetListRoundForCompetitor();
    public Task<List<RoundResponse>> GetListRound(ListModels listModels);
    public Task<RoundResponse?> GetRoundById(Guid id);
    public Task<bool> UpdateRound(RoundUpdateRequest updateRound);
    Task<bool> DeleteRound(Guid id);
    Task<(List<TopicResponse>, int)> GetTopicInRound(Guid id, ListModels listModels);
    Task<(List<RoundResponse>, int)> GetRoundByEducationalLevelId(ListModels listLevelModel, Guid levelId);
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateRoundRequest(RoundRequest round);
    Task<ValidationResult> ValidateRoundUpdateRequest(RoundUpdateRequest roundUpdate);
}