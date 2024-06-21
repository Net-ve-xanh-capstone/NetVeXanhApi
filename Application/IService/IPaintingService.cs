﻿using Application.BaseModels;
using Application.SendModels.Painting;
using Application.ViewModels.PaintingViewModels;
using Infracstructures.SendModels.Painting;

namespace Application.IService;

public interface IPaintingService
{
    Task<bool> AddPainting(PaintingRequest request);
    Task<(List<PaintingViewModel>, int)> GetListPainting(ListModels listPaintingModel);
    Task<bool> DeletePainting(Guid paintingId);
    Task<bool> UpdatePainting(UpdatePaintingViewModel updatePainting);
    Task<PaintingViewModel?> GetPaintingByCode(string code);
    Task<PaintingViewModel?> GetPaintingById(Guid id);
    Task<PaintingViewModel> List20WiningPainting();


    public Task<bool> SubmitPainting(Guid paintingId);
    public Task<PaintingViewModel?> ReviewDecisionOfPainting(PaintingUpdateStatusRequest request);
    public Task<PaintingViewModel?> FinalDecisionOfPainting(PaintingUpdateStatusRequest request);
}