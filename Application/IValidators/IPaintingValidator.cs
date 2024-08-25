using Application.SendModels.Painting;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace Application.IValidators;

public interface IPaintingValidator
{
    IValidator<StaffUpdatePaintingRequest> StaffUpdatePaintingRequestValidator { get; }
    IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }
    IValidator<CompetitorCreatePaintingRequest> CompetitorCreatePaintingRequestValidator { get; }
    IValidator<StaffCreatePaintingRequest> StaffCreatePaintingSendModelValidator { get; }
    IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }
    IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }
    IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }
}