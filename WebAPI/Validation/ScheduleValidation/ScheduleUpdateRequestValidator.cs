﻿using Application.SendModels.AccountSendModels;
using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation
{
    public class ScheduleUpdateRequestValidator : AbstractValidator<ScheduleUpdateRequest>
    {
    }
}