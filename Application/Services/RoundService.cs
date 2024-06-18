﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Round;
using Application.ViewModels.RoundViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;

namespace Application.Services;

public class RoundService : IRoundService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public RoundService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Create

    public async Task<Guid?> CreateRound(RoundRequest Round)
    {
        var newRound = _mapper.Map<Round>(Round);
        newRound.Status = RoundStatus.ACTIVE.ToString();
        await _unitOfWork.RoundRepo.AddAsync(newRound);
        await _unitOfWork.SaveChangesAsync();
        return newRound.Id;
    }

    #endregion

    #region Get All

    public async Task<(List<RoundViewModel>, int)> GetListRound(ListModels listModels)
    {
        var list = await _unitOfWork.RoundRepo.GetAllAsync();
        list = (List<Round>)list.Where(x => x.Status == "ACTIVE");

        var result = new List<Round>();

        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<RoundViewModel>>(result), totalPages);
    }

    #endregion

    #region Get By Id

    public async Task<RoundViewModel?> GetRoundById(Guid id)
    {
        var Round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (Round == null) return null;
        return _mapper.Map<RoundViewModel>(Round);
    }

    #endregion

    #region Update

    public async Task<RoundViewModel?> UpdateRound(RoundUpdateRequest updateRound)
    {
        var Round = await _unitOfWork.RoundRepo.GetByIdAsync(updateRound.Id);
        if (Round == null) return null;

        _mapper.Map(updateRound, Round);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoundViewModel>(Round);
    }

    #endregion

    #region Delete

    public async Task<bool?> DeleteRound(Guid id)
    {
        var Round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (Round == null) return false;

        Round.Status = "INACTIVE";
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    #endregion
}