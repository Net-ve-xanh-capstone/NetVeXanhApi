using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class TopicValidationService : ITopicValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public TopicValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.TopicRepo.IsExistIdAsync(id);
    }
}