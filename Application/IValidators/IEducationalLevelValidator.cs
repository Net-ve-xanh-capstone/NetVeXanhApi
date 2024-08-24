using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace Application.IValidators;

public interface IEducationalLevelValidator
{
    IValidator<EducationalLevelRequest> EducationalLevelRequestValidator { get; }
    IValidator<CreateEducationalLevelRequest> CreateEducationalLevelRequestValidator { get; }
    IValidator<EducationalLevelUpdateRequest> EducationalLevelUpdateRequestValidator { get; }
}