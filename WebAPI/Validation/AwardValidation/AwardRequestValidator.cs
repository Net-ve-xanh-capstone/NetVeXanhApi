using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Award;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class AwardRequestValidator : AbstractValidator<AwardRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    public AwardRequestValidator()
    {
        RuleFor(x => x.Rank)
            .NotEmpty().WithMessage("Rank không được để trống.")
            .Length(1, 50).WithMessage("Rank phải có độ dài từ 1 đến 50 ký tự.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Quantity phải lớn hơn hoặc bằng 1.");

        RuleFor(x => x)
            .Must(model => model.Cash == 0 && model.Artifact == "Không có thông tin")
            .WithMessage("Chỉ 1 trong 2 cash hoặc artifact được trống");

        RuleFor(x => x.Cash)
            .GreaterThanOrEqualTo(0).WithMessage("Cash phải lớn hơn hoặc bằng 0.");

        RuleFor(x => x.EducationalLevelId)
            .NotEmpty().WithMessage("EducationalLevelId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("EducationalLevelId không hợp lệ.");

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