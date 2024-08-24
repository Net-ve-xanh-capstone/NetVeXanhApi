using Application.SendModels.AccountSendModels;
using Application.SendModels.Award;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation
{
    public class CreateDependentAwardRequestValidator : AbstractValidator<CreateDependentAwardRequest>
    {
        public CreateDependentAwardRequestValidator() {
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Số lượng phải lớn hơn hoặc bằng 1.");
        }
    }
}
