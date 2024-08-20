using Application.BaseModels;
using Application.SendModels.Schedule;
using Application.ViewModels.AccountViewModels;
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
    Task<List<ScheduleWebResponse?>> GetScheduleForWeb(Guid examinerId/*, Guid contestId*/);
    Task<bool> RatingPainting(RatingSendModel ratingPainting);

    Task<bool> RatingFinalRound(RatingSendModel ratingPainting);

    Task<bool> RatingQualifyingRound(RatingSendModel ratingPainting);
    Task<bool> RatingFirstPrize(RatingSendModel ratingPainting);
    Task<bool> RatingSecondPrize(RatingSendModel ratingPainting);
    Task<bool> RatingConsolationPrize(RatingSendModel ratingPainting);
    Task<bool> RatingThirdPrize(RatingSendModel ratingPainting);
    Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule);
    Task<bool> DeleteSchedule(Guid id);
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateScheduleRequest(ScheduleForPreliminaryRequest schedule);
    Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate);
    Task<(byte[], string)> GetListCompetitorPass(Guid roundId);
    public Task<List<CompetitorResponse>> GetListCompetitorFinalRound(Guid roundId);
}