using Application.IValidators;
using Application.SendModels.Schedule;
using FluentValidation;

namespace Infracstructures.Validators;

public class ScheduleValidator : IScheduleValidator
{
    public ScheduleValidator(IValidator<ScheduleForPreliminarySendModel> schedulevalidator,
        IValidator<RatingRequest> ratingvalidator,
        IValidator<ScheduleUpdateRequest> scheduleupdatevalidator,
        IValidator<ScheduleForFinalSendModel> scheduleforfinalvalidator)
    {
        ScheduleRequestValidator = schedulevalidator;
        RatingRequestValidator = ratingvalidator;
        ScheduleUpdateRequestValidator = scheduleupdatevalidator;
        ScheduleForFinalRequestValidator = scheduleforfinalvalidator;
    }

    public IValidator<RatingRequest> RatingRequestValidator { get; }

    public IValidator<ScheduleForPreliminarySendModel> ScheduleRequestValidator { get; }
    public IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }

    public IValidator<ScheduleForFinalSendModel> ScheduleForFinalRequestValidator { get; }
}