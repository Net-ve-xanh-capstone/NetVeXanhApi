﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Resources;
using Application.ViewModels.ResourcesViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;

namespace Application.Services;

public class ResourcesService : IResourcesService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public ResourcesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Create

    public async Task<bool> CreateResources(ResourcesRequest Resources)
    {
        var newResources = _mapper.Map<Resources>(Resources);
        newResources.Status = ResourcesStatus.Active.ToString();
        await _unitOfWork.ResourcesRepo.AddAsync(newResources);
        
        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion

    #region Get All

    public async Task<(List<ResourcesViewModel>, int)> GetListResources(ListModels listModels)
    {
        var list = await _unitOfWork.ResourcesRepo.GetAllAsync();
        list = (List<Resources>)list.Where(x => x.Status == "ACTIVE");

        var result = new List<Resources>();

        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ResourcesViewModel>>(result), totalPages);
    }

    #endregion

    #region Get By Id

    public async Task<ResourcesViewModel?> GetResourcesById(Guid id)
    {
        var Resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(id);
        if (Resources == null) return null;
        return _mapper.Map<ResourcesViewModel>(Resources);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateResources(ResourcesUpdateRequest updateResources)
    {
        var Resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(updateResources.Id);
        if (Resources == null) throw new Exception("Khong tim thay Resource");
        _mapper.Map(updateResources, Resources);
        
        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteResources(Guid id)
    {
        var Resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(id);
        if (Resources == null) throw new Exception("Khong tim thay Resource");
        Resources.Status = "INACTIVE";

        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion
}