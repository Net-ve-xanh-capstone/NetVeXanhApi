﻿using Application.BaseModels;
using Application.SendModels.Award;
using Application.ViewModels.AwardViewModels;

namespace Application.IService;

public interface IAwardService
{
    Task<bool> AddAward(AwardRequest addAwardViewModel);
    Task<(List<AwardViewModel>, int)> GetListAward(ListModels listAwardModel);
    Task<bool> DeleteAward(Guid awardId);
    Task<bool> UpdateAward(UpdateAwardRequest updateAward);
    Task<AwardViewModel> GetAwardById(Guid awardId);
}