using Application.BaseModels;
using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using Application.ViewModels.ScheduleViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IScheduleService
{
    Task<List<ListScheduleViewModel>> GetListSchedule(Guid id);
    Task<bool> CreateScheduleForPreliminaryRound(ScheduleRequest Schedule);
    Task<bool> CreateScheduleForFinalRound(ScheduleRequest Schedule);
    Task<(List<ScheduleRatingViewModel>, int)> GetListSchedule(ListModels listModels);
    Task<ScheduleRatingViewModel?> GetScheduleById(Guid id);
    Task<List<ScheduleViewModel?>> GetScheduleByExaminerId(Guid id);
    Task<bool> RatingPreliminaryRound(RatingRequest ratingPainting);
    Task<bool> RatingFirstPrize(RatingRequest ratingPainting);
    Task<bool> RatingSecondPrize(RatingRequest ratingPainting);
    Task<bool> RatingConsolationPrize(RatingRequest ratingPainting);
    Task<bool> RatingThirdPrize(RatingRequest ratingPainting);
    Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule);
    Task<bool> DeleteSchedule(Guid id);
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateScheduleRequest(ScheduleRequest schedule);
    Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate);
    Task<(byte[], string)> GetListCompetitorPass(Guid roundId);
}