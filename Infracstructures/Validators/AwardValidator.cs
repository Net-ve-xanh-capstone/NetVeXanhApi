using Application.IValidators;
using Application.SendModels.Award;
using FluentValidation;

namespace Infracstructures.Validators;

public class AwardValidator : IAwardValidator
{
    public AwardValidator(IValidator<CreateAwardRequest> awardvalidator,
        IValidator<UpdateAwardRequest> updateawardvalidator
        , IValidator<CreateDependentAwardRequest> createdependentawardrequestvalidator
    )
    {
        CreateAwardRequestValidator = awardvalidator;
        UpdateAwardRequestValidator = updateawardvalidator;
        CreateDependentAwardRequestValidator = createdependentawardrequestvalidator;
    }

    public IValidator<CreateAwardRequest> CreateAwardRequestValidator { get; }

    public IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }

    public IValidator<CreateDependentAwardRequest> CreateDependentAwardRequestValidator { get; }
}