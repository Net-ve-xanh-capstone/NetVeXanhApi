using Application;
using Application.SendModels.Award;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class CreateDependentAwardRequestValidator : AbstractValidator<CreateDependentAwardRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public CreateDependentAwardRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng giải phải lớn hơn hoặc bằng 1.");
    }
}