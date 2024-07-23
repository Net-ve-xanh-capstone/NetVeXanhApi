using Application.IService;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class UpdatePaintingRequestValidator : AbstractValidator<UpdatePaintingRequest>
{
    private readonly IAccountService _accountService;
    public UpdatePaintingRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là một GUID hợp lệ.");

        // Validate AwardId
        RuleFor(x => x.AwardId)
            .NotEqual(Guid.Empty).When(x => x.AwardId.HasValue).WithMessage("AwardId phải là một GUID hợp lệ.");

        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEqual(Guid.Empty).When(x => x.RoundTopicId.HasValue).WithMessage("RoundTopicId phải là một GUID hợp lệ.");

        // Validate AccountId
        RuleFor(x => x.AccountId)
            .NotEqual(Guid.Empty).When(x => x.AccountId.HasValue).WithMessage("AccountId phải là một GUID hợp lệ.");

        // Validate ScheduleId
        RuleFor(x => x.ScheduleId)
            .NotEqual(Guid.Empty).When(x => x.ScheduleId.HasValue).WithMessage("ScheduleId phải là một GUID hợp lệ.");

        // Validate Code
        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code phải ít hơn 50 ký tự.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là một GUID hợp lệ.")
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