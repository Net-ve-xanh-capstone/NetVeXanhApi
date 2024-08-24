using Application.SendModels.Schedule;
using FluentValidation;

namespace Application.IValidators;

public interface IScheduleValidator
{
    IValidator<RatingFinalRoundForWeb> RatingFinalRoundForWebValidator { get; }
    IValidator<RatingSendModel> RatingSendModelValidator { get; }
    IValidator<ScheduleForPreliminaryRequest> ScheduleRequestValidator { get; }
    IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }
    IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
}