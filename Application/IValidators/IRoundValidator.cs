using Application.SendModels.Round;
using FluentValidation;

namespace Application.IValidators;

public interface IRoundValidator
{
    IValidator<RoundRequest> RoundRequestValidator { get; }
    IValidator<CreateRoundRequest> CreateRoundRequestValidator { get; }
    IValidator<RoundUpdateRequest> RoundUpdateRequestValidator { get; }
}