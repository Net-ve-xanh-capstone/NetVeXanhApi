using Application.BaseModels;
using Application.IService;
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
                Message = "Get Notification Success",
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
                Message = "Get Notification Success",
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

    #region Send Result Final Round

    [HttpPut("sendresultfinalround/{id}")]
    public async Task<IActionResult> SendResultFinalRound([FromRoute] Guid id)
    {
        try
        {
            var result = await _notificationService.SendResultFinalRound(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Send Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Send Result Preliminary Round

    [HttpPut("sendresultpreliminaryround/{id}")]
    public async Task<IActionResult> SendResultPreliminaryRound([FromRoute] Guid id)
    {
        try
        {
            var result = await _notificationService.SendResultPreliminaryRound(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Send Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion
}