using Application.IService;
using Application.SendModels.Resources;
using FluentValidation;

namespace WebAPI.Validation.ResourceValidation;

public class ResourcesRequestValidator : AbstractValidator<ResourcesRequest>
{
    private readonly IAccountService _accountService;
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