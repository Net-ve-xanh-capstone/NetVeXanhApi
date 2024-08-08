using Application.BaseModels;
using Application.IService;
using Application.SendModels.PaintingCollection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/paintingcollections/")]
public class PaintingCollectionController : Controller
{
    private readonly IPaintingCollectionService _paintingCollectionService;

    public PaintingCollectionController(IPaintingCollectionService paintingCollectionService)
    {
        _paintingCollectionService = paintingCollectionService;
    }


    #region Add Painting To Collection

    [HttpPost("addpaintingtocollection")]
    public async Task<IActionResult> AddPaintingToCollection(PaintingCollectionRequest addPaintingCollectionViewModel)
    {
        try
        {
            var result = await _paintingCollectionService.AddPaintingToCollection(addPaintingCollectionViewModel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Thêm tranh vào bộ sưu tập thành công",
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


    #region Delete Painting Collection

    [HttpDelete("deletepaintingcollection/{id}")]
    public async Task<IActionResult> DeletePainting([FromRoute] Guid id)
    {
        try
        {
            var result = await _paintingCollectionService.DeletePaintingInCollection(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa tranh khỏi bộ sưu tập thành công"
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
}