using Application.IValidators;
using Application.SendModels.Award;
using FluentValidation;

namespace Infracstructures.Validators;

public class AwardValidator : IAwardValidator
{
    public AwardValidator(IValidator<CreateAwardRequest> awardvalidator, IValidator<UpdateAwardRequest> updateAwardvalidator)
    {
        AwardRequestValidator = awardvalidator;
        UpdateAwardRequestValidator = updateAwardvalidator;
    }

    public IValidator<CreateAwardRequest> AwardRequestValidator { get; }

    public IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }
}