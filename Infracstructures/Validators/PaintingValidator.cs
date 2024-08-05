using Application.IValidators;
using Application.SendModels.Painting;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Infracstructures.Validators;

public class PaintingValidator : IPaintingValidator
{
    public PaintingValidator(
        IValidator<StaffUpdatePaintingRequest> staffUpdatePaintingRequestValidator,
        IValidator<StaffCreatePaintingFinalRoundRequest> staffCreatePaintingFinalRoundRequestValidator,
        IValidator<CompetitorCreatePaintingRequest> paintingvalidator,
        IValidator<StaffCreatePaintingRequest> painting2validator,
        IValidator<PaintingUpdateStatusRequest> paintingupdatestatusvalidator,
        IValidator<UpdatePaintingRequest> updatepaintingvalidator,
        IValidator<FilterPaintingRequest> filterpaintingvalidator)
    {
        PaintingRequestValidator = paintingvalidator;
        PaintingRequest2Validator = painting2validator;
        PaintingUpdateStatusRequestValidator = paintingupdatestatusvalidator;
        StaffUpdatePaintingRequestValidator = staffUpdatePaintingRequestValidator;
        StaffCreatePaintingFinalRoundRequestValidator = staffCreatePaintingFinalRoundRequestValidator;
        UpdatePaintingRequestValidator = updatepaintingvalidator;
        FilterPaintingRequestValidator = filterpaintingvalidator;
    }

    public IValidator<CompetitorCreatePaintingRequest> PaintingRequestValidator { get; }

    public IValidator<StaffCreatePaintingRequest> PaintingRequest2Validator { get; }

    public IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }


    public IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }

    public IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }

    public IValidator<StaffUpdatePaintingRequest> StaffUpdatePaintingRequestValidator { get; }

    public IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }
}