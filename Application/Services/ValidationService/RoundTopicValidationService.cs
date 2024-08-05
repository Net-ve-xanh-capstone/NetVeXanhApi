using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class RoundTopicValidationService : IRoundTopicValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoundTopicValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundTopicRepo.IsExistIdAsync(id);
    }
}