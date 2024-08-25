using Application.IValidators;
using Application.SendModels.Contest;
using FluentValidation;

namespace Infracstructures.Validators;

public class ContestValidator : IContestValidator
{
    public ContestValidator(IValidator<ContestRequest> contestvalidator,
        IValidator<UpdateContestRequest> updatecontestvalidator,
        IValidator<CreateContestRequest> createcontestrequestvalidator
    )
    {
        ContestRequestValidator = contestvalidator;
        UpdateContestValidator = updatecontestvalidator;
        CreateContestRequestValidator = createcontestrequestvalidator;
    }

    public IValidator<ContestRequest> ContestRequestValidator { get; }

    public IValidator<UpdateContestRequest> UpdateContestValidator { get; }

    public IValidator<CreateContestRequest> CreateContestRequestValidator { get; }
}