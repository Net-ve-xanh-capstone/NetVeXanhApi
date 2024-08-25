using Application.IValidators;
using Application.SendModels.Collection;
using FluentValidation;

namespace Infracstructures.Validators;

public class CollectionValidator : ICollectionValidator
{
    public CollectionValidator(IValidator<CollectionRequest> collectionvalidator,
        IValidator<UpdateCollectionRequest> updatecollectionvalidator,
        IValidator<CreatePaintingAwardCollectionRequest> createpaintingawardcollectionrequestvalidator
    )
    {
        CollectionRequestValidator = collectionvalidator;
        UpdateCollectionRequestValidator = updatecollectionvalidator;
        CreatePaintingAwardCollectionRequestValidator = createpaintingawardcollectionrequestvalidator;
    }

    public IValidator<CollectionRequest> CollectionRequestValidator { get; }

    public IValidator<UpdateCollectionRequest> UpdateCollectionRequestValidator { get; }

    public IValidator<CreatePaintingAwardCollectionRequest> CreatePaintingAwardCollectionRequestValidator { get; }
}