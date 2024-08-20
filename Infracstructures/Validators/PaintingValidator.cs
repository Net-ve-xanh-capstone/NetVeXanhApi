using Application.IValidators;
using Application.SendModels.Painting;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Infracstructures.Validators;

public class PaintingValidator : IPaintingValidator
{
    public PaintingValidator(
        IValidator<StaffUpdatePaintingSendModel> staffUpdatePaintingRequestValidator,
        IValidator<StaffCreatePaintingFinalRoundRequest> staffCreatePaintingFinalRoundRequestValidator,
        IValidator<CompetitorCreatePaintingRequest> paintingvalidator,
        IValidator<StaffCreatePaintingSendModel> painting2validator,
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

    public IValidator<StaffCreatePaintingSendModel> PaintingRequest2Validator { get; }

    public IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }


    public IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }

    public IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }

    public IValidator<StaffUpdatePaintingSendModel> StaffUpdatePaintingRequestValidator { get; }

    public IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }
}