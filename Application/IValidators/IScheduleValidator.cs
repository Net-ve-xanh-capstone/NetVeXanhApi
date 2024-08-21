using Application.SendModels.Schedule;
using FluentValidation;

namespace Application.IValidators;

public interface IScheduleValidator
{
    IValidator<ScheduleForPreliminaryRequest> ScheduleRequestValidator { get; }
    IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }
    IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
}