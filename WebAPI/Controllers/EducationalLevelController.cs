using Application.BaseModels;
using Application.IService;
using Application.SendModels.EducationalLevel;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    ///     Api Tạo Level (mới)
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateEducationalLevel(CreateEducationalLevelRequest model)
    {
        try
        {
            var result = await _educationalLevelService.CreateEducationalLevel(model);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo đối tượng thành công",
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

    #region Get EducationalLevel By Page
    [Authorize(Roles = "Staff")]
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
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách đối tượng thành công",
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
    [Authorize(Roles = "Staff")]
    [HttpGet("getalllevel")]
    public async Task<IActionResult> GetAllEducationalLevel()
    {
        try
        {
            var result = await _educationalLevelService.GetAllEducationalLevel();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách đối tượng thành công",
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
    [Authorize(Roles = "Staff")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationalLevelById(Guid id)
    {
        try
        {
            var result = await _educationalLevelService.GetEducationalLevelById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin đối tượng thành công",
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
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách đối tượng theo cuộc thi thành công",
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
    [Authorize(Roles = "Staff")]
    [HttpPut]
    public async Task<IActionResult> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel)
    {
        try
        {
            var result = await _educationalLevelService.UpdateEducationalLevel(updateEducationalLevel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa đối tượng thành công"
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

    #region Delete EducationalLevel
    [Authorize(Roles = "Staff")]
    [HttpPatch]
    public async Task<IActionResult> DeleteEducationalLevel(Guid id)
    {
        try
        {
            var result = await _educationalLevelService.DeleteEducationalLevel(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa đối tượng thành công"
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