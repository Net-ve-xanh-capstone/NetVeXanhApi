using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class ScheduleValidationService : IScheduleValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ScheduleRepo.IsExistIdAsync(id);
    }
}