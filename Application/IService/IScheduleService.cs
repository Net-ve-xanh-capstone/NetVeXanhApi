using Application.BaseModels;
using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using Application.ViewModels.ScheduleViewModels;

namespace Application.IService;

public interface IScheduleService
{
    public Task<bool> CreateScheduleForPreliminaryRound(ScheduleRequest Schedule);
    public Task<bool> CreateScheduleForFinalRound(ScheduleRequest Schedule);
    public Task<(List<ScheduleViewModel>, int)> GetListSchedule(ListModels listModels);
    public Task<ScheduleViewModel?> GetScheduleById(Guid id);
    public Task<bool> RatingPreliminaryRound(RatingPreliminaryRound ratingPainting);
    public Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule);
    public Task<bool> DeleteSchedule(Guid id);
}