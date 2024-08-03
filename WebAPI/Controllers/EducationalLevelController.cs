using Application.BaseModels;
using Application.IService;
using Application.SendModels.EducationalLevel;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/educationallevels/")]
public class EducationalLevelController : Controller
{
    private readonly IEducationalLevelService _educationalLevelService;

    public EducationalLevelController(IEducationalLevelService educationalLevelService)
    {
        _educationalLevelService = educationalLevelService;
    }

    #region Create EducationalLevel

    [HttpPost]
    public async Task<IActionResult> CreateEducationalLevel(EducationalLevelRequest educationalLevel)
    {
        try
        {
            var result = await _educationalLevelService.CreateEducationalLevel(educationalLevel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create EducationalLevel Success",
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

    #region Get EducationalLevel By Page

    [HttpGet]
    public async Task<IActionResult> GetEducationalLevelByPage([FromQuery] ListModels listLevelModel)
    {
        try
        {
            var (list, totalPage) = await _educationalLevelService.GetListEducationalLevel(listLevelModel);
            if (totalPage < listLevelModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
                Result = new
                {
                    List = list,
                    TotalPage = totalPage
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<EducationalLevel>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All  EducationalLevel

    [HttpGet("getalllevel")]
    public async Task<IActionResult> GetAllEducationalLevel()
    {
        try
        {
            var result = await _educationalLevelService.GetAllEducationalLevel();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<EducationalLevel>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get EducationalLevel By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationalLevelById(Guid id)
    {
        try
        {
            var result = await _educationalLevelService.GetEducationalLevelById(id);
            if (result == null) return NotFound(new { Success = false, Message = "EducationalLevel not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
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

    #region Get EducationalLevel By ContestId

    [HttpGet("geteducationlevelbycontestid/{id}")]
    public async Task<IActionResult> GetEducationalLevelByContestId([FromQuery] ListModels listLevelModel,
        [FromRoute] Guid id)
    {
        try
        {
            var (list, totalPage) = await _educationalLevelService.GetEducationalLevelByContestId(listLevelModel, id);
            if (totalPage < listLevelModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
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

    #region Update EducationalLevel

    [HttpPut]
    public async Task<IActionResult> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel)
    {
        try
        {
            var result = await _educationalLevelService.UpdateEducationalLevel(updateEducationalLevel);
            if (!result) return NotFound(new { Success = false, Message = "EducationalLevel not found" });
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

    #region Delete EducationalLevel

    [HttpPatch]
    public async Task<IActionResult> DeleteEducationalLevel(Guid id)
    {
        try
        {
            var result = await _educationalLevelService.DeleteEducationalLevel(id);
            if (!result) return NotFound();
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