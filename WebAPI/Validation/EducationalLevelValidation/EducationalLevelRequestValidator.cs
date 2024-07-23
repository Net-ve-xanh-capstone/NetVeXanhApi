using Application.IService;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace WebAPI.Validation.EducationalLevelValidation;

public class EducationalLevelRequestValidator : AbstractValidator<EducationalLevelRequest>
{
    private readonly IAccountService _accountService;
    public EducationalLevelRequestValidator()
    {
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
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là kiểu GUID.")
            .MustAsync(async (userId, cancellation) =>
            {
                try
                {
                    return await _accountService.IsExistedId(userId);
                }
                catch (Exception)
                {
                    // Xử lý lỗi kiểm tra ID
                    return false; // Giả sử ID không tồn tại khi có lỗi
                }
            })
            .WithMessage("CurrentUserId không tồn tại.");
    }
}