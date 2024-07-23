using Application.IService;
using Application.SendModels.Topic;
using FluentValidation;

namespace WebAPI.Validation.TopicValidation;

public class TopicRequestValidator : AbstractValidator<TopicRequest>
{
    private readonly IAccountService _accountService;

    public TopicRequestValidator(IAccountService accountService)
    {
        RuleFor(x => x.Name)
            .Length(2, 50).WithMessage("Name phải có từ 2 tới 50 ký tự.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
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