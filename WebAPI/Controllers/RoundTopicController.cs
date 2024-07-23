using Application.BaseModels;
using Application.IService;
using Application.SendModels.RoundTopic;
using Application.SendModels.Topic;
using Application.Services;
using Domain.Models;
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
                Message = "Get Topic Success",
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
                Message = "Get Topic Success",
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

    [HttpPost]
    public async Task<IActionResult> AddTopicToRound(RoundTopicRequest roundTopicRequest)
    {
        try
        {
            var validationResult = await _roundTopicService.ValidateRoundTopicRequest(roundTopicRequest);
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
            var result = await _roundTopicService.AddTopicToRound(roundTopicRequest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Add RoundTopic Success",
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

    #region Delete Topic In Round

    [HttpDelete("deleteroundtopic")]
    public async Task<IActionResult> DeleteTopicInRound(RoundTopicDeleteRequest roundTopicDeleteRequest)
    {
        try
        {
            var validationResult = await _roundTopicService.ValidateRoundTopicDeleteRequest(roundTopicDeleteRequest);
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
            var result = await _roundTopicService.DeleteTopicInRound(roundTopicDeleteRequest);
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
}