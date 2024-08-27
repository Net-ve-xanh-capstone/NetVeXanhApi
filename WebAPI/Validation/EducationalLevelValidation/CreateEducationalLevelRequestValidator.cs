using Application;
using Application.SendModels.EducationalLevel;
using FluentValidation;
using WebAPI.Validation.RoundValidation;

namespace WebAPI.Validation.EducationalLevelValidation;

public class CreateEducationalLevelRequestValidator : AbstractValidator<CreateEducationalLevelRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public CreateEducationalLevelRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate Level
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .MaximumLength(100).WithMessage("Cấp độ không được dài hơn 100 ký tự.");

        // Validate Description
        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Mô tả không được dài hơn 300 ký tự.");

        // Validate MinAge and MaxAge
        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo(0).WithMessage("Tuổi tối thiểu phải lớn hơn hoặc bằng 0.")
            .LessThanOrEqualTo(120).WithMessage("Tuổi tối thiểu không được lớn hơn 120.");

        RuleFor(x => x.MaxAge)
            .GreaterThanOrEqualTo(x => x.MinAge).WithMessage("Tuổi tối đa phải lớn hơn hoặc bằng tuổi tối thiểu.")
            .LessThanOrEqualTo(120).WithMessage("Tuổi tối đa không được lớn hơn 120.");

        // Validate Round
        RuleFor(x => x.Round)
            .NotNull().WithMessage("Danh sách vòng thi không được để trống.")
            .NotEmpty().WithMessage("Danh sách vòng thi không được rỗng.");

        RuleForEach(x => x.Round)
            .SetValidator(new CreateRoundRequestValidator(_validationServiceManager));
    }
}