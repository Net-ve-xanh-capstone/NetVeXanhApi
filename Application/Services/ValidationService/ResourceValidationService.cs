using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class ResourceValidationService : IResourceValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ResourceValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ResourcesRepo.IsExistIdAsync(id);
    }
}