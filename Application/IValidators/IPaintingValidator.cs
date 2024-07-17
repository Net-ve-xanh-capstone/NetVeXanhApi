﻿using Application.SendModels.Painting;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Application.IValidators;

public interface IPaintingValidator
{
    IValidator<CompetitorCreatePaintingRequest> PaintingRequestValidator { get; }
    IValidator<StaffCreatePaintingRequest> PaintingRequest2Validator { get; }
    IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }
    IValidator<RatingRequest> RatingRequestValidator { get; }
    IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }
    IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }
}