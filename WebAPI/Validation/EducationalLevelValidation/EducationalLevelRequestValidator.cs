using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace WebAPI.Validation.EducationalLevelValidation;

public class EducationalLevelRequestValidator : AbstractValidator<EducationalLevelRequest>
{
    private readonly IAccountValidationService _accountValidationService;

    public EducationalLevelRequestValidator(IAccountValidationService accountValidationService)
    {
        _accountValidationService = accountValidationService;
        // Validate Level
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level không được trống.")
            .Length(1, 50).WithMessage("Level phải có từ 1 tới 50 chữ.");

        // Validate ContestId
        RuleFor(x => x.ContestId)
            .NotEmpty().WithMessage("ContestId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("ContestId phải là kiểu GUID.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
        .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.CurrentUserId.ToString()), () =>
        {
            RuleFor(x => x.CurrentUserId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("CurrentUserId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CurrentUserId)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            try
                            {
                                return await _accountValidationService.IsExistedId(userId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
}