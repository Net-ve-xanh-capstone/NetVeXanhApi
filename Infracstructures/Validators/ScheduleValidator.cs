using Application.IValidators;
using Application.SendModels.Schedule;
using FluentValidation;

namespace Infracstructures.Validators;

public class ScheduleValidator : IScheduleValidator
{
    public ScheduleValidator(IValidator<ScheduleForPreliminaryRequest> schedulevalidator,
        IValidator<RatingSendModel> ratingvalidator,
        IValidator<ScheduleUpdateRequest> scheduleupdatevalidator,
        IValidator<ScheduleForFinalRequest> scheduleforfinalvalidator)
    {
        ScheduleRequestValidator = schedulevalidator;
        RatingRequestValidator = ratingvalidator;
        ScheduleUpdateRequestValidator = scheduleupdatevalidator;
        ScheduleForFinalRequestValidator = scheduleforfinalvalidator;
    }

    public IValidator<RatingSendModel> RatingRequestValidator { get; }

    public IValidator<ScheduleForPreliminaryRequest> ScheduleRequestValidator { get; }
    public IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }

    public IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
}