using Application.BaseModels;
using Application.IService;
using Infracstructures.ViewModels.NotificationViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/notifications/")]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    #region Get 5 Notification

    [HttpGet("get5notification/{id}")]
    public async Task<IActionResult> Get5Notification([FromRoute] Guid id)
    {
        try
        {
            var list = await _notificationService.Get5Notification(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy 5 thông báo thành công",
                Result = new
                {
                    List = list
                }
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion


    #region Get All Notification

    /// <summary>
    ///     Lấy danh sách người thong bao có phân trang
    /// </summary>
    /// <returns></returns>
    [HttpGet("getallnotificationbyacountwithpagination")]
    public async Task<IActionResult> GetAllNotiWithPagination([FromQuery] ListModels listCompetitorModel,
        Guid accountId)
    {
        try
        {
            var (list, totalPage) =
                await _notificationService.GetNotificationByAccountId(listCompetitorModel, accountId);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách thành công",
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
                    List = new List<NotificationResponse>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Notification By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificationById([FromRoute] Guid id)
    {
        try
        {
            var result = await _notificationService.GetNotificationById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Notification not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy chi tiết thông báo thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Read Notification

    [HttpPatch("{id}")]
    public async Task<IActionResult> IsReadNotification([FromRoute] Guid id)
    {
        try
        {
            var result = await _notificationService.ReadNotification(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Successfully"
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion
}