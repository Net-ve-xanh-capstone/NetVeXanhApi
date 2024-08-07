using Application.IValidators;
using Application.SendModels.Schedule;
using FluentValidation;

namespace Infracstructures.Validators;

public class ScheduleValidator : IScheduleValidator
{
    public ScheduleValidator(IValidator<ScheduleRequest> schedulevalidator,
        IValidator<RatingRequest> ratingvalidator,
        IValidator<ScheduleUpdateRequest> scheduleupdatevalidator,
        IValidator<ScheduleForFinalRequest> scheduleforfinalvalidator)
    {
        ScheduleRequestValidator = schedulevalidator;
        RatingRequestValidator = ratingvalidator;
        ScheduleUpdateRequestValidator = scheduleupdatevalidator;
        ScheduleForFinalRequestValidator = scheduleforfinalvalidator;
    }

    public IValidator<RatingRequest> RatingRequestValidator { get; }

    public IValidator<ScheduleRequest> ScheduleRequestValidator { get; }
    public IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }

    public IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
}