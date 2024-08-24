using Application.SendModels.Contest;
using FluentValidation;

namespace Application.IValidators;

public interface IContestValidator
{
    IValidator<ContestRequest> ContestRequestValidator { get; }
    IValidator<CreateContestRequest> CreateContestRequestValidator { get; }
    IValidator<UpdateContestRequest> UpdateContestValidator { get; }
}