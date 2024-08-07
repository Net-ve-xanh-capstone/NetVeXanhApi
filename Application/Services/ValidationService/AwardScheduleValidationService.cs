using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class AwardScheduleValidationService : IAwardScheduleValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public AwardScheduleValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AwardScheduleRepo.IsExistIdAsync(id);
    }
}