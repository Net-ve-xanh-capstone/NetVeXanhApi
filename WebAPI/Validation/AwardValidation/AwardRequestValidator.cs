using Application.IService;
using Application.SendModels.Award;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class AwardRequestValidator : AbstractValidator<AwardRequest>
{
    private readonly IAccountService _accountService;
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
            .NotEmpty().WithMessage("CurrentUserId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId không hợp lệ.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}