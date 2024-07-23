using Application.IService;
using Application.SendModels.Report;
using FluentValidation;

namespace WebAPI.Validation.ReportValidation;

public class UpdateReportRequestValidator : AbstractValidator<UpdateReportRequest>
{
    private readonly IAccountService _accountService;
    public UpdateReportRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được trống.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là kiểu GUID.");

        // Validate Title
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title phải ít hơn 100 chữ.");

        // Validate Description
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description phải ít hơn 500 chữ.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là kiểu GUID.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}