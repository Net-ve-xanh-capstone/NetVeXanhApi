using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Category;
using Application.ViewModels.CategoryViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Add Category

    public async Task<bool> AddCategory(CategoryRequest addCategoryViewModel)
    {
        var validationResult = await ValidateCategoryRequest(addCategoryViewModel);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var category = _mapper.Map<Category>(addCategoryViewModel);

        category.Status = CategoryStatus.Unused.ToString();

        await _unitOfWork.CategoryRepo.AddAsync(category);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete Category

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryId);
        if (category == null) throw new Exception("Khong tim thay Category");
        if (category.Status == CategoryStatus.Unused.ToString())
            await _unitOfWork.CategoryRepo.DeleteAsync(category);
        else
            category.Status = CategoryStatus.Deleted.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Category

    public async Task<bool> UpdateCategory(UpdateCategoryRequest updateCategory)
    {
        var validationResult = await ValidateCategoryUpdateRequest(updateCategory);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(updateCategory.Id);
        if (category == null) throw new Exception("Khong tim thay Category");

        _mapper.Map(updateCategory, category);

        category.UpdatedTime = _currentTime.GetCurrentTime();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region List All Category

    public async Task<(List<CategoryViewModel>, int)> ListCategory(ListModels listCategoryModel)
    {
        var list = await _unitOfWork.CategoryRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Category nao");
        var result = _mapper.Map<List<CategoryViewModel>>(list);

        var totalPages = (int)Math.Ceiling((double)result.Count / listCategoryModel.PageSize);
        int? itemsToSkip = (listCategoryModel.PageNumber - 1) * listCategoryModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listCategoryModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region List All Category

    public async Task<List<CategoryViewModel>> ListAllCategory()
    {
        var list = await _unitOfWork.CategoryRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Category nao");
        var result = _mapper.Map<List<CategoryViewModel>>(list);

        return result;
    }

    #endregion

    #region List Category Unused With Pagination

    public async Task<(List<CategoryViewModel>, int)> ListCategoryUnused(ListModels listCategoryModel)
    {
        var list = await _unitOfWork.CategoryRepo.GetCategoryUnused();
        if (list.Count == 0) throw new Exception("Khong co Category nao dang khong duoc su dung");
        var result = _mapper.Map<List<CategoryViewModel>>(list);

        var totalPages = (int)Math.Ceiling((double)result.Count / listCategoryModel.PageSize);
        int? itemsToSkip = (listCategoryModel.PageNumber - 1) * listCategoryModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listCategoryModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion


    #region List Category Used With Pagination

    public async Task<(List<CategoryViewModel>, int)> ListCategoryUsed(ListModels listCategoryModel)
    {
        var list = await _unitOfWork.CategoryRepo.GetCategoryUsed();
        if (list.Count == 0) throw new Exception("Khong co Category nao dang duoc su dung");
        var result = _mapper.Map<List<CategoryViewModel>>(list);

        var totalPages = (int)Math.Ceiling((double)result.Count / listCategoryModel.PageSize);
        int? itemsToSkip = (listCategoryModel.PageNumber - 1) * listCategoryModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listCategoryModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region List All Category Unused

    public async Task<List<CategoryViewModel>> ListAllCategoryUnused()
    {
        var list = await _unitOfWork.CategoryRepo.GetCategoryUnused();
        if (list.Count == 0) throw new Exception("Khong co Category nao dang khong duoc su dung");
        var result = _mapper.Map<List<CategoryViewModel>>(list);
        return result;
    }

    #endregion


    #region List All Category Used With Pagination

    public async Task<List<CategoryViewModel>> ListAllCategoryUsed()
    {
        var list = await _unitOfWork.CategoryRepo.GetCategoryUsed();
        if (list.Count == 0) throw new Exception("Khong co Category nao dang duoc su dung");
        var result = _mapper.Map<List<CategoryViewModel>>(list);
        return result;
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.CategoryRepo.IsExistIdAsync(id);
    }

    #region Validate

    public async Task<ValidationResult> ValidateCategoryRequest(CategoryRequest category)
    {
        return await _validatorFactory.CategoryRequestValidator.ValidateAsync(category);
    }

    public async Task<ValidationResult> ValidateCategoryUpdateRequest(UpdateCategoryRequest categoryUpdate)
    {
        return await _validatorFactory.UpdateCategoryRequestValidator.ValidateAsync(categoryUpdate);
    }

    #endregion
}