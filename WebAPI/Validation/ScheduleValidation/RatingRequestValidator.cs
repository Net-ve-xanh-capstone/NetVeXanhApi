using Application;
using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

public class RatingRequestValidator : AbstractValidator<RatingRequest>
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


       
    }
}