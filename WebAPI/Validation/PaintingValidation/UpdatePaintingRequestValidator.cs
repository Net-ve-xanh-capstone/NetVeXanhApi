using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class UpdatePaintingRequestValidator : AbstractValidator<UpdatePaintingRequest>
{
    private readonly IAccountValidationService _accountValidationService;
    private readonly IPaintingValidationService _paintingValidationService;

    public UpdatePaintingRequestValidator(IAccountValidationService accountValidationService, IPaintingValidationService paintingValidationService)
    {
        _accountValidationService = accountValidationService;
        _paintingValidationService = paintingValidationService;
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
                                return await _paintingValidationService.IsExistedId(topicId);
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