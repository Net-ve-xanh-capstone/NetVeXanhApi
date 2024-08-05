using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Collection;
using Application.ViewModels.CollectionViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class CollectionService : ICollectionService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public CollectionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Add Collection

    public async Task<bool> AddCollection(CollectionRequest addCollectionViewModel)
    {
        var validationResult = await ValidateCollectionRequest(addCollectionViewModel);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var collection = _mapper.Map<Collection>(addCollectionViewModel);
        collection.Status = CollectionStatus.Active.ToString();
        await _unitOfWork.CollectionRepo.AddAsync(collection);
        await _unitOfWork.SaveChangesAsync();
        //add Painting
        var listPaintingCollection = new List<PaintingCollection>();
        foreach (var paintingId in addCollectionViewModel.listPaintingId)
            listPaintingCollection.Add(new PaintingCollection
            {
                PaintingId = paintingId,
                CollectionId = collection.Id
            });
        await _unitOfWork.PaintingCollectionRepo.AddRangeAsync(listPaintingCollection);
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
        var validationResult = await ValidateCollectionUpdateRequest(updateCollection);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
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

    public async Task<GetPaintingInCollection> GetPaintingByCollection(ListModels listPaintingModel,
        Guid collectionId)
    {
        var collection = await _unitOfWork.CollectionRepo.GetPaintingByCollectionAsync(collectionId);
        if (collection == null) throw new Exception("Khong co Painting nao trong Collection");

        return _mapper.Map<GetPaintingInCollection>(collection);
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

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.CollectionRepo.IsExistIdAsync(id);
    }

    #region Add Collection with award Painting in Contest

    public async Task<bool> AddCollectionWithPaintingAwardInContest(
        CreatePaintingAwardCollectionRequest addCollectionViewModel)
    {
        var collection = _mapper.Map<Collection>(addCollectionViewModel);
        collection.Status = CollectionStatus.Active.ToString();
        await _unitOfWork.CollectionRepo.AddAsync(collection);
        await _unitOfWork.SaveChangesAsync();
        //add Painting
        var listPaintingCollection = new List<PaintingCollection>();

        await _unitOfWork.PaintingCollectionRepo.AddRangeAsync(listPaintingCollection);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Validate

    public async Task<ValidationResult> ValidateCollectionRequest(CollectionRequest collection)
    {
        return await _validatorFactory.CollectionRequestValidator.ValidateAsync(collection);
    }

    public async Task<ValidationResult> ValidateCollectionUpdateRequest(UpdateCollectionRequest collectionUpdate)
    {
        return await _validatorFactory.UpdateCollectionRequestValidator.ValidateAsync(collectionUpdate);
    }

    #endregion
}