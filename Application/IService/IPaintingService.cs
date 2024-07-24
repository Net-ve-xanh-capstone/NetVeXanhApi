using Application.BaseModels;
using Application.SendModels.Painting;
using Application.ViewModels.PaintingViewModels;
using Infracstructures.SendModels.Painting;

namespace Application.IService;

public interface IPaintingService
{
    Task<bool> UpdatePainting(UpdatePaintingRequest updatePainting);
    Task<bool> UpdatePaintingStaffPermisson(UpdatePaintingRequest updatePainting);

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
    Task<bool> IsExistedId(Guid id);
    Task<PaintingViewModel> GetPaintingByAccountContest(Guid contestId, Guid AccountId);
}