using Application.IService;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class CompetitorCreatePaintingRequestValidator : AbstractValidator<CompetitorCreatePaintingRequest>
{
    private readonly IAccountService _accountService;
    public CompetitorCreatePaintingRequestValidator()
    {
        // Validate AccountId
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("AccountId phải là một GUID hợp lệ.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("AccountId không tồn tại.");

        // Validate Image
        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Hình ảnh là bắt buộc.");

        // Validate Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên phải ít hơn 100 ký tự.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.")
            .MaximumLength(250).WithMessage("Mô tả phải ít hơn 250 ký tự.");

        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEmpty().WithMessage("RoundTopicId là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("RoundTopicId phải là một GUID hợp lệ.");
    }

}