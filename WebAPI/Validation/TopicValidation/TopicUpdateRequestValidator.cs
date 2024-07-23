using Application.IService;
using Application.SendModels.Topic;
using FluentValidation;

namespace WebAPI.Validation.TopicValidation;

public class TopicUpdateRequestValidator : AbstractValidator<TopicUpdateRequest>
{
    private readonly IAccountService _accountService;
    public TopicUpdateRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được trống.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là kiểu GUID.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty()
            .WithMessage("CurrentUserId không được trống.")
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