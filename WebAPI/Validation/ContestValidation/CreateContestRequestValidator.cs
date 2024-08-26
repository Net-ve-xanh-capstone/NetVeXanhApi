using Application;
using Application.SendModels.Contest;
using FluentValidation;
using WebAPI.Validation.EducationalLevelValidation;

namespace WebAPI.Validation.ContestValidation;

public class CreateContestRequestValidator : AbstractValidator<CreateContestRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public CreateContestRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(100).WithMessage("Tên không được dài hơn 100 ký tự.");

        // Validate StartTime 
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime).WithMessage("Thời gian bắt đầu cuộc thi phải trước thời gian kết thúc.");


        // Validate Content
        RuleFor(x => x.Content)
            .MaximumLength(1000).WithMessage("Nội dung không được dài hơn 1000 ký tự.");

        RuleFor(x => x.CreatedBy)
        .NotEmpty().WithMessage("CreatedBy không được để trống.");

            When(x => x.CreatedBy.HasValue, () =>
            {
                RuleFor(x => x.CreatedBy.Value)
                    .Must(createdBy => Guid.TryParse(createdBy.ToString(), out _))
                    .WithMessage("CreatedBy phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.CreatedBy.Value)
                            .MustAsync(async (createdBy, cancellation) =>
                            {
                                return await _validationServiceManager.AccountValidationService
                                    .IsExistedId(createdBy);
                            })
                            .WithMessage("CreatedBy không tồn tại.");
                    });
            });

        // Validate EducationalLevel
        RuleFor(x => x.EducationalLevel)
            .NotNull().WithMessage("Danh sách cấp độ giáo dục không được để trống.")
            .NotEmpty().WithMessage("Danh sách cấp độ giáo dục không được rỗng.");

        RuleForEach(x => x.EducationalLevel)
            .SetValidator(new CreateEducationalLevelRequestValidator(_validationServiceManager));
    }
}