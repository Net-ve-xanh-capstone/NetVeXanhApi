using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.PaintingCollection;
using Application.SendModels.Topic;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;

namespace Application.Services;

public class PaintingCollectionService : IPaintingCollectionService
{
    private readonly IAuthentication _authentication;
    private readonly IClaimsService _claimsService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public PaintingCollectionService(IUnitOfWork unitOfWork, IAuthentication authentication, IMapper mapper,
        IMailService mailService, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _claimsService = claimsService;
        _mailService = mailService;
        _authentication = authentication;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    public async Task<bool> AddPaintingToCollection(PaintingCollectionRequest addPaintingCollectionViewModel)
    {
        var validationResult = await ValidatePaintingCollectionRequest(addPaintingCollectionViewModel);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var paintingCollection = _mapper.Map<PaintingCollection>(addPaintingCollectionViewModel);
        await _unitOfWork.PaintingCollectionRepo.AddAsync(paintingCollection);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletePaintingInCollection(Guid paintingcollectionId)
    {
        var paintingcollection = await _unitOfWork.PaintingCollectionRepo.GetByIdAsync(paintingcollectionId);
        if (paintingcollection == null) throw new Exception("Khong tim thay PaintingCollection");
        await _unitOfWork.PaintingCollectionRepo.DeleteAsync(paintingcollection);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }



    #region Validate
    
    public async Task<ValidationResult> ValidatePaintingCollectionRequest(PaintingCollectionRequest paintingcollection)
    {
        return await _validatorFactory.PaintingCollectionRequestValidator.ValidateAsync(paintingcollection);
    }

    #endregion
}