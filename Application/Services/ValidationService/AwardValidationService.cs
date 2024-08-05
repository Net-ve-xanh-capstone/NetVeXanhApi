using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class AwardValidationService : IAwardValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public AwardValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AwardRepo.IsExistIdAsync(id);
    }
}