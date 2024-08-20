using Application.BaseModels;
using Application.SendModels.EducationalLevel;
using Application.ViewModels.EducationalLevelViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IEducationalLevelService
{
    Task<bool> CreateEducationalLevel(CreateEducationalLevelRequest model);
    Task<(List<EducationalLevelResponse>, int)> GetListEducationalLevel(ListModels listModels);
    Task<List<EducationalLevelResponse>> GetAllEducationalLevel();
    Task<EducationalLevelResponse?> GetEducationalLevelById(Guid id);

    Task<(List<EducationalLevelResponse>, int)> GetEducationalLevelByContestId(ListModels listLevelModel,
        Guid contestId);

    Task<bool> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel);
    Task<bool> DeleteEducationalLevel(Guid id);

    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateLevelRequest(EducationalLevelRequest level);

    Task<ValidationResult> ValidateLevelUpdateRequest(EducationalLevelUpdateRequest levelUpdate);
}