using Application.IService.ICommonService;
using Domain.Enums;
using Infracstructures;

namespace Application.Services.CommonService;

public class SchedulerTrigger : ISchedulerTrigger
{
    private readonly IUnitOfWork _unitOfWork;

    public SchedulerTrigger(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ScheduleTrigger()
    {
        Console.WriteLine("Check");
        /*await OutDateSchedule();
        await Contest();
        await Round();
        await _unitOfWork.SaveChangesAsync();*/
    }
    
    public async Task OutDateSchedule()
    {
        var end = await _unitOfWork.ScheduleRepo.SchedulerTrigger();
        if (end.Any())
        {
            end.ForEach(src => src.Status = ScheduleStatus.OutOfDate.ToString());
            end.ToList().ForEach(src => src.AwardSchedule.ToList().ForEach(aw => aw.Status = AwardScheduleStatus.OutOfDate.ToString()));
            end.ToList().ForEach(src => src.Painting.ToList().ForEach(aw => aw.ScheduleId = null));
            _unitOfWork.ScheduleRepo.UpdateRange(end);
        }
    }

    public async Task Contest()
    {
        var end = await _unitOfWork.ContestRepo.EndContest();
        if (end.Any())
        {
            end.ToList().ForEach(src => src.Status = ContestStatus.Inactive.ToString());
            _unitOfWork.ContestRepo.UpdateRange(end);
        }

        var start = await _unitOfWork.ContestRepo.StartContest();
        if (start.Any())
        {
            start.ToList().ForEach(src => src.Status = ContestStatus.Active.ToString());
            _unitOfWork.ContestRepo.UpdateRange(start);
        }
    }

    public async Task Round()
    {
        var end = await _unitOfWork.RoundRepo.EndRound();
        if (end.Any())
        {
            end.ToList().ForEach(src => src.Status = RoundStatus.Inactive.ToString());
            _unitOfWork.RoundRepo.UpdateRange(end);
        }
        var start = await _unitOfWork.RoundRepo.StartRound();
        if (start.Any())
        {
            start.ToList().ForEach(src => src.Status = RoundStatus.Active.ToString());
            _unitOfWork.RoundRepo.UpdateRange(start);
        }
    }
    
}