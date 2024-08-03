using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Application.IValidators;

public interface IPaintingValidator
{
    IValidator<StaffUpdatePaintingRequest> StaffUpdatePaintingRequestValidator { get; }
    IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }
    IValidator<CompetitorCreatePaintingRequest> PaintingRequestValidator { get; }
    IValidator<StaffCreatePaintingRequest> PaintingRequest2Validator { get; }
    IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }
    IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }
    IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }
}