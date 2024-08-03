using Application.BaseModels;
using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using Application.ViewModels.PaintingViewModels;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures.SendModels.Painting;

namespace Application.IService;

public interface IPaintingService
{
    Task<bool> UpdatePainting(UpdatePaintingRequest updatePainting);
    Task<bool> UpdatePaintingStaffPermission(StaffUpdatePaintingRequest updatePainting);

    Task<(List<PaintingViewModel>, int)> GetListPainting(ListModels listPaintingModel);
    Task<PaintingViewModel?> GetPaintingByCode(string code);
    Task<PaintingViewModel?> GetPaintingById(Guid id);
    Task<List<PaintingViewModel>> List16WiningPainting();

    Task<(List<PaintingViewModel>, int)> FilterPainting(FilterPaintingRequest filterPainting,
        ListModels listPaintingModel);

    Task<(List<PaintingViewModel>, int)> ListPaintingByAccountId(Guid accountId, ListModels listPaintingModel);

    #region Competitor

    Task<bool> DraftPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request);
    Task<bool> SubmitPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request);
    Task<bool> DeletePainting(Guid paintingId);

    #endregion

    #region Staff

    public Task<PaintingViewModel?> ReviewDecisionOfPainting(PaintingUpdateStatusRequest request);
    public Task<PaintingViewModel?> FinalDecisionOfPainting(PaintingUpdateStatusRequest request);
    public Task<bool> StaffSubmitPaintingForPreliminaryRound(StaffCreatePaintingRequest request);
    public Task<bool> StaffSubmitPaintingForFinalRound(StaffCreatePaintingFinalRoundRequest request);

    #endregion
    Task<PaintingTrackingViewModel> PaintingTracking(Guid id);
    Task<PaintingViewModel> GetPaintingByAccountContest(Guid contestId, Guid AccountId);

    Task<ValidationResult> ValidateCompetitorCreateRequest(CompetitorCreatePaintingRequest painting);
    Task<ValidationResult> ValidateFilterPaintingRequest(FilterPaintingRequest filterPainting);
    Task<ValidationResult> ValidatePaintingUpdateStatusRequest(PaintingUpdateStatusRequest painting);
    Task<ValidationResult> ValidateStaffCreateRequest(StaffCreatePaintingRequest painting);
    Task<ValidationResult> ValidateUpdatePaintingRequest(UpdatePaintingRequest painting);
}