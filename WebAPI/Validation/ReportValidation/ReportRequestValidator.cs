using Application.IService;
using Application.SendModels.Report;
using FluentValidation;

namespace WebAPI.Validation.ReportValidation;

public class ReportRequestValidator : AbstractValidator<ReportRequest>
{
    private readonly IAccountService _accountService;
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