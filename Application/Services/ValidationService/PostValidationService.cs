using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class PostValidationService : IPostValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.PostRepo.IsExistIdAsync(id);
    }
}