using Application.BaseModels;
using Application.IService;
using Application.SendModels.Collection;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/collections/")]
public class CollectionController : Controller
{
    private readonly ICollectionService _collectionService;

    public CollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }


    #region Create Collection

    [HttpPost]
    public async Task<IActionResult> CreateCollection(CollectionRequest collection)
    {
        try
        {
            var result = await _collectionService.AddCollection(collection);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo bộ sưu tập thành công",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            // Tạo danh sách các thông điệp lỗi từ ex.Errors
            var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();

            // Kết hợp tất cả các thông điệp lỗi thành một chuỗi duy nhất với các dòng mới
            var combinedErrorMessage = string.Join("  |  ", errorMessages);
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = combinedErrorMessage,
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

    #region Update Collection

    [HttpPut]
    public async Task<IActionResult> UpdateCollection(UpdateCollectionRequest updateCollection)
    {
        try
        {
            var result = await _collectionService.UpdateCollection(updateCollection);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Thay đổi thông tin bộ sưu tập thành công"
            });
        }
        catch (ValidationException ex)
        {
            // Tạo danh sách các thông điệp lỗi từ ex.Errors
            var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();

            // Kết hợp tất cả các thông điệp lỗi thành một chuỗi duy nhất với các dòng mới
            var combinedErrorMessage = string.Join("  |  ", errorMessages);
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = combinedErrorMessage,
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

    #region Delete Collection

    [HttpPatch]
    public async Task<IActionResult> DeleteCollection(Guid id)
    {
        try
        {
            var result = await _collectionService.DeleteCollection(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa bộ sưu tập thành công"
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

    #region Get Collection By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCollectionById([FromRoute] Guid id)
    {
        try
        {
            var result = await _collectionService.GetCollectionById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy chi tiết bộ sưu tập thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Painting By Collection

    [HttpGet("Painting/{id}")]
    public async Task<IActionResult> GetPaintingByCollection([FromRoute] Guid id,
        [FromQuery] ListModels listPaintingmodel)
    {
        try
        {
            var result = await _collectionService.GetPaintingByCollection(listPaintingmodel, id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy tranh trong bộ sưu tập thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Collection>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Collection

    [HttpGet("getallcollection")]
    public async Task<IActionResult> GetAllCollection([FromQuery] ListModels listPaintingmodel)
    {
        try
        {
            var (list, totalPage) = await _collectionService.GetAllCollection(listPaintingmodel);
            if (totalPage < listPaintingmodel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách bộ sưu tập thành công",
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
                    List = new List<Collection>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Collection By AccountId

    [HttpGet("getcollectionbyaccountid/{id}")]
    public async Task<IActionResult> GetCollectionByAccountId([FromRoute] Guid id,
        [FromQuery] ListModels listPaintingmodel)
    {
        try
        {
            var (list, totalPage) = await _collectionService.GetCollectionByAccountId(listPaintingmodel, id);
            if (totalPage < listPaintingmodel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách bộ sưu tập theo tài khoản thành công",
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
                    List = new List<Collection>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get 6 Collection tạo bởi Staff (gần nhất)

    [HttpGet("get6staffcollection")]
    public async Task<IActionResult> Get6CollectionCreatedByStaff()
    {
        try
        {
            var result = await _collectionService.Get6StaffCollection();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy 6 bộ sưu tập của nhân viên thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Collection>(),
                Errors = ex
            });
        }
    }

    #endregion
}