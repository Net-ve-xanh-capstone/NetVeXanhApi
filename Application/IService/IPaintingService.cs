using Application.BaseModels;
using Application.SendModels.Painting;
using Application.ViewModels.PaintingViewModels;
using FluentValidation.Results;
using Infracstructures.SendModels.Painting;

namespace Application.IService;

public interface IPaintingService
{
    Task<bool> UpdatePainting(UpdatePaintingRequest updatePainting);
    Task<bool> UpdatePaintingStaffPermission(StaffUpdatePaintingRequest updatePainting);

    Task<(List<PaintingResponse>, int)> GetListPainting(ListModels listPaintingModel);
    Task<PaintingResponse?> GetPaintingByCode(string code);
    Task<PaintingResponse?> GetPaintingById(Guid id);
    Task<List<PaintingForScheduleResponse>> GetPaintingByScheduleId(Guid scheduleId);
    Task<List<PaintingResponse>> List16WiningPainting();

    Task<(List<PaintingResponse>, int)> FilterPainting(FilterPaintingRequest filterPainting,
        ListModels listPaintingModel);

    Task<(List<PaintingResponse>, int)> ListPaintingByAccountId(Guid accountId, ListModels listPaintingModel);
    Task<PaintingTrackingResponse> PaintingTracking(Guid id);
    Task<PaintingResponse> GetPaintingByAccountContest(Guid contestId, Guid AccountId);

    Task<ValidationResult> ValidateCompetitorCreateRequest(CompetitorCreatePaintingRequest painting);
    Task<ValidationResult> ValidateFilterPaintingRequest(FilterPaintingRequest filterPainting);
    Task<ValidationResult> ValidatePaintingUpdateStatusRequest(PaintingUpdateStatusRequest painting);
    Task<ValidationResult> ValidateStaffCreateRequest(StaffCreatePaintingRequest painting);
    Task<ValidationResult> ValidateUpdatePaintingRequest(UpdatePaintingRequest painting);

    #region Competitor

    Task<bool> DraftPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request);
    Task<bool> SubmitPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request);
    Task<bool> DeletePainting(Guid paintingId);

    #endregion

    #region Staff

    public Task<PaintingResponse?> ReviewDecisionOfPainting(PaintingUpdateStatusRequest request);
    public Task<PaintingResponse?> FinalDecisionOfPainting(PaintingUpdateStatusRequest request);
    public Task<bool> StaffSubmitPaintingForPreliminaryRound(StaffCreatePaintingRequest request);
    public Task<bool> StaffSubmitPaintingForFinalRound(StaffCreatePaintingFinalRoundRequest request);

    #endregion
}