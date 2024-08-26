using Application.BaseModels;
using Application.SendModels.Schedule;
using Application.ViewModels.ScheduleViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IScheduleService
{
    Task<List<ListScheduleResponse>> GetListScheduleByContestId(Guid id);
    Task<bool> CreateScheduleForQualifyingRound(ScheduleForPreliminaryRequest schedule);
    Task<bool> CreateScheduleForFinal(ScheduleForFinalRequest schedule);
    Task<(List<ScheduleRatingResponse>, int)> GetListSchedule(ListModels listModels);
    Task<ScheduleRatingResponse?> GetScheduleById(Guid id);
    Task<List<ScheduleResponse?>> GetScheduleByExaminerId(Guid id);
    Task<List<ScheduleWebResponse?>> GetScheduleForWeb(Guid examinerId /*, Guid contestId*/);
    Task<bool> RatingPainting(RatingSendModel ratingPainting);
    Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule);
    Task<bool> DeleteSchedule(Guid id);
    Task<bool> IsExistedId(Guid id);
    Task<bool> ConfirmRating(Guid id);
    Task<ValidationResult> ValidateScheduleForPreliminaryRequest(ScheduleForPreliminaryRequest schedule);
    Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate);
}