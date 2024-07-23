using Application.IService;
using Application.IService.IValidationService;
using Application.IValidators;
using Application.SendModels.Contest;
using FluentValidation;

namespace WebAPI.Validation.ContestValidation;

public class UpdateContestValidator : AbstractValidator<UpdateContest>
{
    private readonly IAccountValidationService _accountValidationService;
    private readonly IContestValidationService _contestValidationService;

    public UpdateContestValidator(IAccountValidationService accountValidationService, IContestValidationService contestValidationService)
    {
        _accountValidationService = accountValidationService;
        _contestValidationService = contestValidationService;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(topicId => Guid.TryParse(topicId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (topicId, cancellation) =>
                        {
                            try
                            {
                                return await _contestValidationService.IsExistedId(topicId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });
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