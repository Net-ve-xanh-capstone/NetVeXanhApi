using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class PaintingValidationService : IPaintingValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaintingValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.PaintingRepo.IsExistIdAsync(id);
    }

    //Check is Exist Painting In Contest
    public async Task<bool> IsExistedPaintingInContest(Guid accountId, Guid roundtopicId)
    {
        var roundtopic = await _unitOfWork.RoundTopicRepo.GetByIdAsync(roundtopicId);
        if (roundtopic == null) throw new Exception("Không tìm thấy roundtopic");
        return await _unitOfWork.PaintingRepo.IsExistPaintingInContest(accountId, roundtopic.RoundId.Value);
    }
}