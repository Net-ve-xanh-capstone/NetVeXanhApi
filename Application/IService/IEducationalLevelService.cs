using Application.BaseModels;
using Application.SendModels.EducationalLevel;
using Application.ViewModels.EducationalLevelViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IEducationalLevelService
{
    Task<bool> CreateEducationalLevel(EducationalLevelRequest EducationalLevel);
    Task<(List<EducationalLevelViewModel>, int)> GetListEducationalLevel(ListModels listModels);
    Task<List<EducationalLevelViewModel>> GetAllEducationalLevel();
    Task<EducationalLevelViewModel?> GetEducationalLevelById(Guid id);

    Task<(List<EducationalLevelViewModel>, int)> GetEducationalLevelByContestId(ListModels listLevelModel,
        Guid contestId);

    Task<bool> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel);
    Task<bool> DeleteEducationalLevel(Guid id);

    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateLevelRequest(EducationalLevelRequest level);

    Task<ValidationResult> ValidateLevelUpdateRequest(EducationalLevelUpdateRequest levelUpdate);
}