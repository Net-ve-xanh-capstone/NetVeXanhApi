using Application.SendModels.Collection;
using FluentValidation;

namespace Application.IValidators;

public interface ICollectionValidator
{
    IValidator<CollectionRequest> CollectionRequestValidator { get; }
    IValidator<CreatePaintingAwardCollectionRequest> CreatePaintingAwardCollectionRequestValidator { get; }
    IValidator<UpdateCollectionRequest> UpdateCollectionRequestValidator { get; }
}