using Application.BaseModels;
using Application.SendModels.Category;
using Application.ViewModels.CategoryViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface ICategoryService
{
    Task<bool> AddCategory(CategoryRequest addCategoryViewModel);
    Task<bool> DeleteCategory(Guid collectionId);
    Task<bool> UpdateCategory(UpdateCategoryRequest updateCategory);
    Task<(List<CategoryResponse>, int)> ListCategory(ListModels listCategoryModel);
    Task<List<CategoryResponse>> ListAllCategory();
    Task<(List<CategoryResponse>, int)> ListCategoryUnused(ListModels listCategoryModel);
    Task<(List<CategoryResponse>, int)> ListCategoryUsed(ListModels listCategoryModel);
    Task<List<CategoryResponse>> ListAllCategoryUnused();
    Task<List<CategoryResponse>> ListAllCategoryUsed();
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateCategoryRequest(CategoryRequest category);
    Task<ValidationResult> ValidateCategoryUpdateRequest(UpdateCategoryRequest categoryUpdate);
}