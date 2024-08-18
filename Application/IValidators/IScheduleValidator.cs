using Application.SendModels.Schedule;
using FluentValidation;

namespace Application.IValidators;

public interface IScheduleValidator
{
    IValidator<ScheduleForPreliminarySendModel> ScheduleRequestValidator { get; }
    IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }
    IValidator<ScheduleForFinalSendModel> ScheduleForFinalRequestValidator { get; }
}