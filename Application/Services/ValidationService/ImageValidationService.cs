using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class ImageValidationService : IImageValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ImageValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ImageRepo.IsExistIdAsync(id);
    }
}