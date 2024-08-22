using Application.BaseModels;
using Application.IService;
using Application.SendModels.Schedule;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/schedules/")]
public class ScheduleController : Controller
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }


    #region Create Schedule For Qualifying Round
    /// <summary>
    /// Tạo lịch chấm
    /// </summary>
    /// <param name="schedule">    
    ///  <br>AwardCount là số lượng của giải mà giám khảo được chấm</br>
    /// <br>JudgeCount là số lượng mà giám khảo được phân công chấm</br>
    /// </param>
    /// <returns></returns>
    [HttpPost("preliminary")]
    
    public async Task<IActionResult> CreateScheduleForQualifyingRound(ScheduleForPreliminaryRequest schedule)
    {
        try
        {
            var validationResult = await _scheduleService.ValidateScheduleRequest(schedule);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new BaseFailedResponseModel
                {
                    Status = 400,
                    Message = "Validation failed",
                    Result = false,
                    Errors = errors
                };
                return BadRequest(response);
            }
            var result = await _scheduleService.CreateScheduleForQualifyingRound(schedule);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo lịch chấm thành công",
                Result = result
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

    #region Create Schedule For Final Round
    /// <summary>
    /// Tạo lịch chấm
    /// </summary>
    /// <param name="schedule">    
    ///  <br>AwardCount là số lượng của giải mà giám khảo được chấm</br>
    /// <br>JudgeCount là số lượng mà giám khảo được phân công chấm</br>
    /// </param>
    /// <returns></returns>
    [HttpPost("final")]
    public async Task<IActionResult> CreateScheduleForFinalRound(ScheduleForFinalRequest schedule)
    {
        try
        {
            /*var validationResult = await _scheduleService.ValidateScheduleRequest(schedule);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new BaseFailedResponseModel
                {
                    Status = 400,
                    Message = "Validation failed",
                    Result = false,
                    Errors = errors
                };
                return BadRequest(response);
            }*/
            var result = await _scheduleService.CreateScheduleForFinal(schedule);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Hệ thống bị lỗi vui lòng thử lại"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo lịch chấm thành công",
                Result = result
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

    #region Get Schedule By Page

    [HttpGet]
    public async Task<IActionResult> GetScheduleByPage([FromQuery] ListModels listScheduleModel)
    {
        try
        {
            var (list, totalPage) = await _scheduleService.GetListSchedule(listScheduleModel);
            if (totalPage < listScheduleModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách lịch chấm thành công",
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
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Schedule By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleById([FromRoute] Guid id)
    {
        try
        {
            var result = await _scheduleService.GetScheduleById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy chi tiết lịch chấm thành công",
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

    #region Get Schedule By ContestId

    [HttpGet("contestId/{id}")]
    public async Task<IActionResult> GetScheduleByContestId([FromRoute] Guid id)
    {
        try
        {
            var result = await _scheduleService.GetListScheduleByContestId(id);
            if (result == null) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách lịch chấm theo cuộc thi thành công",
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

    #region Update Schedule

    [HttpPut]
    public async Task<IActionResult> UpdateSchedule(ScheduleUpdateRequest updateSchedule)
    {
        try
        {
            var result = await _scheduleService.UpdateSchedule(updateSchedule);
            if (!result) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa lịch chấm thành công"
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

    #region Delete Schedule

    [HttpDelete]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        try
        {
            var result = await _scheduleService
                .DeleteSchedule(id);
            if (result == false) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa lịch chấm thành công"
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

    #region Get Schedule for examiner by examiner Id for Web
    /*/contest/{contestId}*/
    [HttpGet("examiner/{examinerId}")]
    public async Task<IActionResult> GetScheduleForWeb([FromRoute] Guid examinerId/*, [FromRoute] Guid contestId*/)
    {
        try
        {
            var result = await _scheduleService.GetScheduleForWeb(examinerId/*, contestId*/);
            if (result == null) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách lịch chấm theo giám khảo thành công",
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

    #region Get Schedule for examiner by examiner Id

    [HttpGet("/examiner/{id}")]
    public async Task<IActionResult> GetScheduleByExaminerId([FromRoute] Guid id)
    {
        try
        {
            var result = await _scheduleService.GetScheduleByExaminerId(id);
            if (result == null) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách lịch chấm theo giám khảo thành công",
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

    // #region Get List Accoutn Pass
    //
    // [HttpGet("export-round-results")]
    // public async Task<IActionResult> ExportRound1Results(Guid roundId)
    // {
    //     var result = await _scheduleService.GetListCompetitorPass(roundId);
    //     var list = result.Item1;
    //     var name = $"{result.Item2}.xlsx";
    //     return File(list, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name);
    // }
    //
    // #endregion
    //
    // #region Get Schedule for examiner by examiner Id
    //
    // [HttpGet("/finalround/{id}")]
    // public async Task<IActionResult> GetListCompetitorFinalRound([FromRoute] Guid id)
    // {
    //     try
    //     {
    //         var result = await _scheduleService.GetListCompetitorFinalRound(id);
    //         if (result == null) return NotFound(new { Success = false, Message = "Lịch chấm không tìm thấy" });
    //         return Ok(new BaseResponseModel
    //         {
    //             Status = Ok().StatusCode,
    //             Message = "Lấy danh sách lịch chấm thành công",
    //             Result = result
    //         });
    //     }
    //     catch (Exception ex)
    //     {
    //         return Ok(new BaseFailedResponseModel
    //         {
    //             Status = Ok().StatusCode,
    //             Message = ex.Message,
    //             Result = null,
    //             Errors = ex
    //         });
    //     }
    // }
    //
    // #endregion
    
    #region  Rating 
    /// <summary>
    /// Chấm điểm 
    /// </summary>
    /// <param name="rating">không có award thì cho awardId = null</param>
    /// <returns></returns>
    [HttpPut("Rating")]
    public async Task<IActionResult> RatingPainting(RatingSendModel rating)
    {
        try
        {
            var result = await _scheduleService.RatingPainting(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Chấm điểm thành công",
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
    
}