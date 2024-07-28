using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Schedule;
using Application.Services.ValidationService;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

public class ScheduleUpdateRequestValidator : AbstractValidator<ScheduleUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public ScheduleUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
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
                                return await _validationServiceManager.ScheduleValidationService.IsExistedId(topicId);
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
        RuleFor(review => review.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");


        RuleFor(review => review.EndDate)
            .GreaterThan(DateTime.Now).WithMessage("Ngày kết thúc phải lớn hơn ngày hiện tại");


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
                                return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
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