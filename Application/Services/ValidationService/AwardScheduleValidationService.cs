using Application.IService.IValidationService;
using DocumentFormat.OpenXml.Office2010.Excel;

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

    public async Task<bool> IsValidAwardJudge(int JudgeCount,Guid awardId)
    {
        var listAwardSchedule = await _unitOfWork.AwardScheduleRepo.GetByAwardIdAsync(awardId);
        var count = 0;
        if (listAwardSchedule != null) {
            foreach (var a in listAwardSchedule)
            {
                count = count + a.Quantity;
            }
        }
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);

        return award.Quantity >= count + JudgeCount;
    }
}