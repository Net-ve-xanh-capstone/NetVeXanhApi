﻿using Application.SendModels.Contest;
using FluentValidation;

namespace Application.IValidators;

public interface IContestValidator
{
    IValidator<ContestRequest> ContestRequestValidator { get; }
    IValidator<UpdateContest> UpdateContestValidator { get; }
}