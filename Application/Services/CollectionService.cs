﻿using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Collection;
using Application.ViewModels.CollectionViewModels;
using Application.ViewModels.PaintingViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class CollectionService : ICollectionService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CollectionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
    }

    #region Add Collection

    public async Task<bool> AddCollection(CollectionRequest addCollectionViewModel)
    {
        var collection = _mapper.Map<Collection>(addCollectionViewModel);
        collection.Status = CollectionStatus.Active.ToString();
        await _unitOfWork.CollectionRepo.AddAsync(collection);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete Collection

    public async Task<bool> DeleteCollection(Guid collectionId)
    {
        var collection = await _unitOfWork.CollectionRepo.GetByIdAsync(collectionId);
        if (collection == null) throw new Exception("Khong tim thay Collection");


        collection.Status = CollectionStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Collection

    public async Task<bool> UpdateCollection(UpdateCollectionRequest updateCollection)
    {
        var collection = await _unitOfWork.CollectionRepo.GetByIdAsync(updateCollection.Id);
        if (collection == null) throw new Exception("Khong tim thay Collection");
        ;

        /*collection.Name = updateCollection.Name;
        collection.Description = updateCollection.Description;
        collection.Image = updateCollection.Image;*/
        collection = _mapper.Map<Collection>(updateCollection);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Collection By Id

    public async Task<CollectionViewModel> GetCollectionById(Guid collectionId)
    {
        var collection = await _unitOfWork.CollectionRepo.GetByIdAsync(collectionId);
        if (collection == null) throw new Exception("Khong tim thay Collection");
        return _mapper.Map<CollectionViewModel>(collection);
    }

    #endregion

    #region Get Painting By Collection

    public async Task<(List<PaintingViewModel>, int)> GetPaintingByCollection(ListModels listPaintingModel,
        Guid collectionId)
    {
        var listPainting = await _unitOfWork.CollectionRepo.GetPaintingByCollectionAsync(collectionId);
        if (listPainting.Count == 0) throw new Exception("Khong co Painting nao trong Collection");
        var result = _mapper.Map<List<PaintingViewModel>>(listPainting);


        var totalPages = (int)Math.Ceiling((double)result.Count / listPaintingModel.PageSize);
        int? itemsToSkip = (listPaintingModel.PageNumber - 1) * listPaintingModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listPaintingModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Get All Collection

    public async Task<(List<CollectionViewModel>, int)> GetAllCollection(ListModels listCollectionModel)
    {
        var listCollection = await _unitOfWork.CollectionRepo.GetAllAsync();
        if (listCollection.Count == 0) throw new Exception("Khong co Collection nao");
        var result = _mapper.Map<List<CollectionViewModel>>(listCollection);

        var totalPages = (int)Math.Ceiling((double)result.Count / listCollectionModel.PageSize);
        int? itemsToSkip = (listCollectionModel.PageNumber - 1) * listCollectionModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listCollectionModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Get Collection By AccountId

    public async Task<(List<CollectionViewModel>, int)> GetCollectionByAccountId(ListModels listCollectionModel,
        Guid accountId)
    {
        var listCollection = await _unitOfWork.CollectionRepo.GetCollectionByAccountIdAsync(accountId);
        if (listCollection.Count == 0) throw new Exception("Khong co Collection nao");
        var result = _mapper.Map<List<CollectionViewModel>>(listCollection);

        var totalPages = (int)Math.Ceiling((double)result.Count / listCollectionModel.PageSize);
        int? itemsToSkip = (listCollectionModel.PageNumber - 1) * listCollectionModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listCollectionModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Get 6 Staff Collection

    public async Task<List<CollectionPaintingViewModel>> Get6StaffCollection()
    {
        var listCollection = await _unitOfWork.CollectionRepo.GetCollectionsWithStaffAccountsAsync();
        if (listCollection.Count == 0) throw new Exception("Không có Collection nào tạo bởi Staff");
        var result = _mapper.Map<List<CollectionPaintingViewModel>>(listCollection);

        return result;
    }

    #endregion
}