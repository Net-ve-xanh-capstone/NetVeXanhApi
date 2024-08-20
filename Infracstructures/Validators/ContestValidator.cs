using Application.IValidators;
using Application.SendModels.Contest;
using FluentValidation;

namespace Infracstructures.Validators;

public class ContestValidator : IContestValidator
{
    public ContestValidator(IValidator<ContestRequest> contestvalidator,
        IValidator<UpdateContestRequest> updatecontestvalidator)
    {
        ContestRequestValidator = contestvalidator;
        UpdateContestValidator = updatecontestvalidator;
    }

    public IValidator<ContestRequest> ContestRequestValidator { get; }

    public IValidator<UpdateContestRequest> UpdateContestValidator { get; }
}