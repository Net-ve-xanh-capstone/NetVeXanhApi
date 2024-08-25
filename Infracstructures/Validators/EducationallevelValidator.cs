using Application.IValidators;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace Infracstructures.Validators;

public class EducationalLevelValidator : IEducationalLevelValidator
{
    public EducationalLevelValidator(IValidator<EducationalLevelRequest> levelvalidator,
        IValidator<EducationalLevelUpdateRequest> updatelevelvalidator,
        IValidator<CreateEducationalLevelRequest> createeducationallevelrequestvalidator
    )
    {
        EducationalLevelRequestValidator = levelvalidator;
        EducationalLevelUpdateRequestValidator = updatelevelvalidator;
        CreateEducationalLevelRequestValidator = createeducationallevelrequestvalidator;
    }

    public IValidator<EducationalLevelRequest> EducationalLevelRequestValidator { get; }

    public IValidator<EducationalLevelUpdateRequest> EducationalLevelUpdateRequestValidator { get; }

    public IValidator<CreateEducationalLevelRequest> CreateEducationalLevelRequestValidator { get; }
}