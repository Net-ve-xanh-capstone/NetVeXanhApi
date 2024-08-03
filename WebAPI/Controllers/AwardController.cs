using Application.BaseModels;
using Application.IService;
using Application.SendModels.Award;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/awards/")]
public class AwardController : Controller
{
    private readonly IAwardService _awardService;

    public AwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    #region Create Award

    [HttpPost]
    public async Task<IActionResult> CreateAward(AwardRequest award)
    {
        try
        {
            if (!Enum.IsDefined(typeof(RankAward), award.Rank))
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Rank is not Exist!"
                });
            var result = await _awardService.AddAward(award);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Award Success",
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

    #region Update Award

    [HttpPut]
    public async Task<IActionResult> UpdateAward(UpdateAwardRequest updateAward)
    {
        try
        {
            var result = await _awardService.UpdateAward(updateAward);
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

    #region Delete Award

    [HttpPatch]
    public async Task<IActionResult> DeleteAward(Guid id)
    {
        try
        {
            var result = await _awardService.DeleteAward(id);
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

    #region Get All Award

    [HttpGet]
    public async Task<IActionResult> GetAllAward([FromQuery] ListModels listAwardModel)
    {
        try
        {
            var (list, totalPage) = await _awardService.GetListAward(listAwardModel);
            if (totalPage < listAwardModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Award Success",
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
                    List = new List<Award>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Award By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAwardById(Guid id)
    {
        try
        {
            var result = await _awardService.GetAwardById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Award Success",
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
}