using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Report;
using FluentValidation;

namespace WebAPI.Validation.ReportValidation;

public class ReportRequestValidator : AbstractValidator<ReportRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    public ReportRequestValidator()
    {
        // Validate Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title không được trống.")
            .MaximumLength(100).WithMessage("Title phải ít hơn 100 chữ.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description không được trống.")
            .MaximumLength(500).WithMessage("Description phải ít hơn 500 chữ.");

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