using Application.IValidators;
using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Infracstructures.Validators;

public class PaintingValidator : IPaintingValidator
{
    public PaintingValidator(IValidator<CompetitorCreatePaintingRequest> paintingvalidator,
        IValidator<StaffCreatePaintingRequest> painting2validator,
        IValidator<PaintingUpdateStatusRequest> paintingupdatestatusvalidator,
        IValidator<UpdatePaintingRequest> updatepaintingvalidator,
        IValidator<FilterPaintingRequest> filterpaintingvalidator)
    {
        PaintingRequestValidator = paintingvalidator;
        PaintingRequest2Validator = painting2validator;
        PaintingUpdateStatusRequestValidator = paintingupdatestatusvalidator;

        UpdatePaintingRequestValidator = updatepaintingvalidator;
        FilterPaintingRequestValidator = filterpaintingvalidator;
    }

    public IValidator<CompetitorCreatePaintingRequest> PaintingRequestValidator { get; }

    public IValidator<StaffCreatePaintingRequest> PaintingRequest2Validator { get; }

    public IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }



    public IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }

    public IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }
}