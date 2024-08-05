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


    #region Create Schedule For Preliminary Round

    [HttpPost("preliminary")]
    public async Task<IActionResult> CreateScheduleForPreliminaryRound(ScheduleRequest schedule)
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
            var result = await _scheduleService.CreateScheduleForPreliminaryRound(schedule);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Schedule Success",
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

    [HttpPost("final")]
    public async Task<IActionResult> CreateScheduleForFinalRound(ScheduleRequest schedule)
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
            var result = await _scheduleService.CreateScheduleForFinalRound(schedule);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Schedule Success",
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
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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
            var result = await _scheduleService.GetListSchedule(id);
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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
            if (!result) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Update Successfully"
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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
                Message = "Delete Successfully"
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

    [HttpGet("examiner/{examinerId}/")]
    public async Task<IActionResult> GetScheduleForWeb([FromRoute] Guid examinerId)
    {
        try
        {
            var result = await _scheduleService.GetScheduleForWeb(examinerId);
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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

    #region Get List Accoutn Pass

    [HttpGet("export-round-results")]
    public async Task<IActionResult> ExportRound1Results(Guid roundId)
    {
        var result = await _scheduleService.GetListCompetitorPass(roundId);
        var list = result.Item1;
        var name = $"{result.Item2}.xlsx";
        return File(list, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name);
    }

    #endregion

    #region Get Schedule for examiner by examiner Id

    [HttpGet("/finalround/{id}")]
    public async Task<IActionResult> GetListCompetitorFinalRound([FromRoute] Guid id)
    {
        try
        {
            var result = await _scheduleService.GetListCompetitorFinalRound(id);
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Schedule Success",
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

    #region Rating

    [HttpPost("RatingPreliminaryRound")]
    public async Task<IActionResult> RatingPreliminaryRound(RatingRequest rating)
    {
        try
        {
            var result = await _scheduleService.RatingPreliminaryRound(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Rating Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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

    [HttpPost("RatingFirstPrize")]
    public async Task<IActionResult> RatingFirstPrize(RatingRequest rating)
    {
        try
        {
            var result = await _scheduleService.RatingFirstPrize(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Rating Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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

    [HttpPost("RatingSecondPrize")]
    public async Task<IActionResult> RatingSecondPrize(RatingRequest rating)
    {
        try
        {
            var result = await _scheduleService.RatingSecondPrize(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Rating Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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

    [HttpPost("RatingThirdPrize")]
    public async Task<IActionResult> RatingThirdPrize(RatingRequest rating)
    {
        try
        {
            var result = await _scheduleService.RatingThirdPrize(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Rating Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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

    [HttpPost("RatingConsolationPrize")]
    public async Task<IActionResult> RatingConsolationPrize(RatingRequest rating)
    {
        try
        {
            var result = await _scheduleService.RatingConsolationPrize(rating);
            if (result == false)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "There is a certain painting that has an inappropriate status"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Rating Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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