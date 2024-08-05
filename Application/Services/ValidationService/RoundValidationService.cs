using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class RoundValidationService : IRoundValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoundValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundRepo.IsExistIdAsync(id);
    }
}