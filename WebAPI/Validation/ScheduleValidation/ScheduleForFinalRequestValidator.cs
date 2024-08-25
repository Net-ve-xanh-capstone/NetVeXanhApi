using Application;
using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

public class ScheduleForFinalRequestValidator : AbstractValidator<ScheduleForFinalRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public ScheduleForFinalRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(review => review.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");

        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("RoundId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.RoundId.ToString()), () =>
        {
            RuleFor(x => x.RoundId)
                .Must(roundId => Guid.TryParse(roundId.ToString(), out _))
                .WithMessage("RoundId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundId)
                        .MustAsync(async (roundId, cancellation) =>
                        {
                            return await _validationServiceManager.RoundValidationService.IsExistedId(roundId);
                        })
                        .WithMessage("RoundId không tồn tại.");
                    RuleFor(x => x.RoundId)
                        .MustAsync(async (roundId, cancellation) =>
                        {
                            // Kiểm tra trạng thái của RoundId
                            return await _validationServiceManager.RoundValidationService.BeInCompleteStatus(roundId);
                        })
                        .WithMessage("Vòng thi phải hoàn thành mới có thể tạo lịch chấm.");
                });
        });

        RuleFor(x => x.JudgeCount)
            .GreaterThan(0).WithMessage("Số lượng bài chấm phải lớn hơn 0.")
            .MustAsync(async (dto, judgeCount, cancellation) =>
                await _validationServiceManager.PaintingValidationService.NumberJudgeValid(judgeCount, dto.RoundId))
            .WithMessage("Số lượng bài chấm không được vượt quá số lượng tranh chưa được lên lịch chấm.");


        RuleForEach(x => x.Awards)
            .MustAsync(async (award, cancellation) =>
                await _validationServiceManager.AwardScheduleValidationService.IsValidAwardJudge(award.AwardCount, award.AwardId))
            .WithMessage("Số lượng giải đang vượt quá số lượng cho phép.");

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
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
}