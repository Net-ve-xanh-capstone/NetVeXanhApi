using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Category;
using FluentValidation;

namespace WebAPI.Validation.CategoryValidation;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    public CategoryRequestValidator()
    {
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
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .Length(2, 50).WithMessage("Tên phải có độ dài từ 2 đến 50 ký tự.");
    }
}