using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class CompetitorCreatePaintingRequestValidator : AbstractValidator<CompetitorCreatePaintingRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    public CompetitorCreatePaintingRequestValidator()
    {
        // Validate AccountId
        RuleFor(x => x.AccountId)
        .NotEmpty().WithMessage("AccountId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.AccountId.ToString()), () =>
        {
            RuleFor(x => x.AccountId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("AccountId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.AccountId)
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
                        .WithMessage("AccountId không tồn tại.");
                });
        });

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