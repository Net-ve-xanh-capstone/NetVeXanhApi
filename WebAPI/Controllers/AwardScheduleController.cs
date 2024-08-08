using Application.BaseModels;
using Application.IService;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/awardschedules/")]
public class AwardScheduleController : Controller
{
    private readonly IAwardScheduleService _awardSchedule;

    public AwardScheduleController(IAwardScheduleService awardSchedule)
    {
        _awardSchedule = awardSchedule;
    }

    #region Get list Award Schedule By Schedule Id

    [HttpGet("schedule/{id}")]
    public async Task<IActionResult> GetListAwardScheduleById(Guid id)
    {
        try
        {
            var result = await _awardSchedule.GetListByScheduleId(id);
            if (result == null) return NotFound(new { Success = false, Message = "Không tìm thấy lịch chấm" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách lịch chấm thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Lấy danh sách lịch chấm thất bại",
                Result = new List<AwardSchedule>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get AwardSchedule By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAwardById(Guid id)
    {
        try
        {
            var result = await _awardSchedule.GetById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin lịch chấm thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = null,
                Errors = ex
            });
        }
    }

    #endregion
}