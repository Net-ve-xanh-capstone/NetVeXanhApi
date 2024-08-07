using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class CategoryValidationService : ICategoryValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.CategoryRepo.IsExistIdAsync(id);
    }
}