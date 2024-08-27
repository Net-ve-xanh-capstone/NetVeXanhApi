using Application.BaseModels;
using Application.IService;
using Application.SendModels.Category;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/categories/")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    
    #region Create Category
    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryRequest category)
    {
        try
        {
            var result = await _categoryService.AddCategory(category);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo mới danh mục thành công",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var firstErrorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage;
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = firstErrorMessage,
                Result = false
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Category
    [Authorize(Roles = "Staff")]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest updateCategory)
    {
        try
        {
            var result = await _categoryService.UpdateCategory(updateCategory);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa danh mục thành công"
            });
        }
        catch (ValidationException ex)
        {
            var firstErrorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage;
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = firstErrorMessage,
                Result = false
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Delete Category
    [Authorize(Roles = "Staff")]
    [HttpPatch]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa danh mục thành công"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region List Category

    /// <summary>
    ///     Lấy category có phân trang
    /// </summary>
    [HttpGet("getcategory")]
    public async Task<IActionResult> ListCategory([FromQuery] ListModels listCategoryModel)
    {
        try
        {
            var (list, totalPage) = await _categoryService.ListCategory(listCategoryModel);
            if (totalPage < listCategoryModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = new
                {
                    List = list,
                    TotalPage = totalPage
                }
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List All Category

    /// <summary>
    ///     Lấy category không phân trang
    /// </summary>
    [HttpGet("getallcategory")]
    public async Task<IActionResult> ListAllCategory()
    {
        try
        {
            var result = await _categoryService.ListAllCategory();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List Category Unused With Pagination

    /// <summary>
    ///     Lấy danh sách danh mục đã sử dụng
    /// </summary>
    [HttpGet("getcategoryunused")]
    public async Task<IActionResult> ListCategoryUnused([FromQuery] ListModels listCategoryModel)
    {
        try
        {
            var (list, totalPage) = await _categoryService.ListCategoryUnused(listCategoryModel);
            if (totalPage < listCategoryModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = new
                {
                    List = list,
                    TotalPage = totalPage
                }
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List Category Used With Pagination

    /// <summary>
    ///     Lấy danh sách danh mục đã sử dụng có phân trang
    /// </summary>
    [HttpGet("getcategoryused")]
    public async Task<IActionResult> ListCategoryUsed([FromQuery] ListModels listCategoryModel)
    {
        try
        {
            var (list, totalPage) = await _categoryService.ListCategoryUsed(listCategoryModel);
            if (totalPage < listCategoryModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = new
                {
                    List = list,
                    TotalPage = totalPage
                }
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List All Category Unused

    [HttpGet("getallcategoryunused")]
    public async Task<IActionResult> ListAllCategoryUnused()
    {
        try
        {
            var result = await _categoryService.ListAllCategoryUnused();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List All Category Used

    /// <summary>
    ///     Lấy danh mục đã được sử dụng
    /// </summary>
    /// <returns></returns>
    [HttpGet("getallcategoryused")]
    public async Task<IActionResult> ListAllCategoryUsed()
    {
        try
        {
            var result = await _categoryService.ListAllCategoryUsed();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách danh mục thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Category>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion
}