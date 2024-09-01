using Application;
using Application.SendModels.Round;
using FluentValidation;
using WebAPI.Validation.AwardValidation;

namespace WebAPI.Validation.RoundValidation;

public class CreateRoundRequestValidator : AbstractValidator<CreateRoundRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public CreateRoundRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(100).WithMessage("Tên không được dài hơn 100 ký tự.");

        // Validate StartTime and EndTime
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime).WithMessage("Thời gian bắt đầu vòng phải trước thời gian kết thúc.");

        /*// Validate Award
        RuleFor(x => x.Award)
            .NotNull().WithMessage("Danh sách giải thưởng không được để trống.")
            .NotEmpty().WithMessage("Danh sách giải thưởng không được rỗng.");

        RuleForEach(x => x.Award)
            .SetValidator(new CreateDependentAwardRequestValidator(_validationServiceManager));*/
    }
}