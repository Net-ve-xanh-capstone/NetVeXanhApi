using Application.IValidators;
using Application.SendModels.Round;
using FluentValidation;

namespace Infracstructures.Validators;

public class RoundValidator : IRoundValidator
{
    public RoundValidator(IValidator<RoundRequest> roundvalidator, IValidator<RoundUpdateRequest> updateroundvalidator
        , IValidator<CreateRoundRequest> createroundrequestvalidator)
    {
        RoundRequestValidator = roundvalidator;
        RoundUpdateRequestValidator = updateroundvalidator;
        CreateRoundRequestValidator = createroundrequestvalidator;
    }

    public IValidator<RoundRequest> RoundRequestValidator { get; }

    public IValidator<RoundUpdateRequest> RoundUpdateRequestValidator { get; }

    public IValidator<CreateRoundRequest> CreateRoundRequestValidator { get; }
}