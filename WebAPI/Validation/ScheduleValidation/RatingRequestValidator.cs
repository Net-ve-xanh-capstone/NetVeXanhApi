using Application;
using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

public class RatingRequestValidator : AbstractValidator<RatingSendModel>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public RatingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate ScheduleId
        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId không được trống.");
        When(x => !string.IsNullOrEmpty(x.ScheduleId.ToString()), () =>
        {
            RuleFor(x => x.ScheduleId)
                .Must(scheduleId => Guid.TryParse(scheduleId.ToString(), out _))
                .WithMessage("ScheduleId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ScheduleId)
                        .MustAsync(async (scheduleId, cancellation) =>
                        {
                            return await _validationServiceManager.ScheduleValidationService
                                .IsExistedId(scheduleId);
                        })
                        .WithMessage("ScheduleId không tồn tại.");
                });
        });

        // Validate each Painting
        RuleForEach(x => x.Paintings)
            .SetValidator(new PaintingRatingViewModelValidator(_validationServiceManager));
    }
}

public class PaintingRatingViewModelValidator : AbstractValidator<PaintingRatingViewModel>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public PaintingRatingViewModelValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate PaintingId
        RuleFor(x => x.PaintingId)
            .NotEmpty().WithMessage("PaintingId không được trống.");
        When(x => !string.IsNullOrEmpty(x.PaintingId.ToString()), () =>
        {
            RuleFor(x => x.PaintingId)
                .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                .WithMessage("PaintingId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PaintingId)
                        .MustAsync(async (paintingId, cancellation) =>
                        {
                            return await _validationServiceManager.PaintingValidationService
                                .IsExistedId(paintingId);
                        })
                        .WithMessage("PaintingId không tồn tại.");
                });
        });

        // Validate AwardId
        When(x => x.AwardId.HasValue, () =>
        {
            RuleFor(x => x.AwardId)
                .Must(awardId => Guid.TryParse(awardId.ToString(), out _))
                .WithMessage("AwardId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.AwardId.Value)
                        .MustAsync(async (awardId, cancellation) =>
                        {
                            return await _validationServiceManager.AwardValidationService
                                .IsExistedId(awardId);
                        })
                        .WithMessage("AwardId không tồn tại.");
                });
        });

        // Validate Reason
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Lý do phải ít hơn 500 chữ.");
    }
}