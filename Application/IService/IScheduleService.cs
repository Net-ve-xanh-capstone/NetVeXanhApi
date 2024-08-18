using Application.BaseModels;
using Application.SendModels.Schedule;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ScheduleViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IScheduleService
{
    Task<List<ListScheduleViewModel>> GetListScheduleByContestId(Guid id);
    Task<bool> CreateScheduleForPreliminaryRound(ScheduleForPreliminarySendModel schedule);
    Task<bool> CreateScheduleForFinal(ScheduleForFinalSendModel schedule);
    Task<(List<ScheduleRatingViewModel>, int)> GetListSchedule(ListModels listModels);
    Task<ScheduleRatingViewModel?> GetScheduleById(Guid id);
    Task<List<ScheduleViewModel?>> GetScheduleByExaminerId(Guid id);
    Task<List<ScheduleWebViewModel?>> GetScheduleForWeb(Guid examinerId/*, Guid contestId*/);
    Task<bool> RatingPainting(RatingRequest ratingPainting);

    Task<bool> RatingFinalRound(RatingRequest ratingPainting);

    Task<bool> RatingPreliminaryRound(RatingRequest ratingPainting);
    Task<bool> RatingFirstPrize(RatingRequest ratingPainting);
    Task<bool> RatingSecondPrize(RatingRequest ratingPainting);
    Task<bool> RatingConsolationPrize(RatingRequest ratingPainting);
    Task<bool> RatingThirdPrize(RatingRequest ratingPainting);
    Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule);
    Task<bool> DeleteSchedule(Guid id);
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateScheduleRequest(ScheduleForPreliminarySendModel schedule);
    Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate);
    Task<(byte[], string)> GetListCompetitorPass(Guid roundId);
    public Task<List<CompetitorViewModel>> GetListCompetitorFinalRound(Guid roundId);
}