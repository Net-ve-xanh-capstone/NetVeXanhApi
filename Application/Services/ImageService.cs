using Application.BaseModels;
using Application.IService;
using Application.SendModels.Image;
using AutoMapper;
using Domain.Models;
using FluentValidation.Results;
using Infracstructures.ViewModels.ImageViewModels;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public ImageService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreateImage(ImageRequest Image)
    {
        var newImage = _mapper.Map<Image>(Image);
        await _unitOfWork.ImageRepo.AddAsync(newImage);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All

    public async Task<(List<ImageResponse>, int)> GetListImage(ListModels listModels)
    {
        var list = await _unitOfWork.ImageRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Không tìm thấy ảnh");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ImageResponse>>(result), totalPages);
    }

    #endregion

    #region Get By Id

    public async Task<ImageResponse?> GetImageById(Guid id)
    {
        var image = await _unitOfWork.ImageRepo.GetByIdAsync(id);
        if (image == null) throw new Exception("Không tìm thấy ảnh");
        return _mapper.Map<ImageResponse>(image);
    }

    #endregion


    #region Delete

    public async Task<bool> DeleteImage(Guid id)
    {
        var image = await _unitOfWork.PaintingCollectionRepo.GetByIdAsync(id);
        if (image == null) throw new Exception("Khong tim thay Image");
        await _unitOfWork.PaintingCollectionRepo.DeleteAsync(image);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ImageRepo.IsExistIdAsync(id);
    }

    #region Validate

    public async Task<ValidationResult> ValidateImageRequest(ImageRequest image)
    {
        return await _validatorFactory.ImageRequestValidator.ValidateAsync(image);
    }

    #endregion
}