using Application.BaseModels;
using Application.IService;
using Application.SendModels.RoundTopic;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/roundtopics/")]
public class RoundTopicController : ControllerBase
{
    private readonly IRoundTopicService _roundTopicService;

    public RoundTopicController(IRoundTopicService roundTopicService)
    {
        _roundTopicService = roundTopicService;
    }

    #region Get List

    [HttpGet("getalltopic")]
    public async Task<IActionResult> GetAll([FromQuery] GetListRoundTopicRequest request)
    {
        try
        {
            var result = await _roundTopicService.GetListRoundTopicForCompetitor(request);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đề thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Topic>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get List

    [HttpGet("getallroundtopic")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _roundTopicService.GetAll();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đề thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Topic>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get List

    [HttpGet("roundtopic/roundid/{id}")]
    public async Task<IActionResult> GetAll(Guid id)
    {
        try
        {
            var result = await _roundTopicService.GetListRoundTopicForStaff(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đề thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Topic>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Add Topic To Round
[Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> AddTopicToRound(RoundTopicRequest roundTopicRequest)
    {
        try
        {
            var result = await _roundTopicService.AddTopicToRound(roundTopicRequest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Thêm chủ đề vào vòng thi thành công",
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

    #region Delete Topic In Round
    [Authorize(Roles = "Staff")]
    [HttpDelete("deleteroundtopic")]
    public async Task<IActionResult> DeleteTopicInRound(RoundTopicDeleteRequest roundTopicDeleteRequest)
    {
        try
        {
            var result = await _roundTopicService.DeleteTopicInRound(roundTopicDeleteRequest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa chủ đề trong vòng thi thành công"
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
}