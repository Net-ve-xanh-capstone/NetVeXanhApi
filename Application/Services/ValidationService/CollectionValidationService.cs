using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class CollectionValidationService : ICollectionValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public CollectionValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.CollectionRepo.IsExistIdAsync(id);
    }
}