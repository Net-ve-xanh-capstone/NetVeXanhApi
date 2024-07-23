using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Resources;
using FluentValidation;

namespace WebAPI.Validation.ResourceValidation;

public class ResourcesRequestValidator : AbstractValidator<ResourcesRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    public ResourcesRequestValidator()
    {
        // Validate Sponsorship
        RuleFor(x => x.Sponsorship)
            .NotEmpty().WithMessage("Sponsorship không được trống.")
            .MaximumLength(200).WithMessage("Sponsorship phải ít hơn 200 chữ.");

        // Validate SponsorId
        RuleFor(x => x.SponsorId)
            .NotEmpty().WithMessage("SponsorId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("SponsorId phải là kiểu GUID.");

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