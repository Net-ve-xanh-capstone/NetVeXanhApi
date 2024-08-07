﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Round;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/rounds/")]
public class RoundController : Controller
{
    private readonly IRoundService _roundService;

    public RoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    #region Create Round

    [HttpPost]
    public async Task<IActionResult> CreateRound(CreateRoundSendModel model)
    {
        try
        {
            var result = await _roundService.CreateRound(model);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo vòng thi mới thành công",
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

    #region Get All Round

    [HttpGet("getallround")]
    public async Task<IActionResult> GetAllRound([FromQuery] ListModels listRoundModel)
    {
        try
        {
            var result = await _roundService.GetListRound(listRoundModel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách các vòng thi thành công",
                Result = result
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
                    List = new List<Round>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Round By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoundById([FromRoute] Guid id)
    {
        try
        {
            var result = await _roundService.GetRoundById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Không tìm thấy vòng thi" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy chi tiết vòng thi thành công",
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

    #region Update Round

    [HttpPut]
    public async Task<IActionResult> UpdateRound(RoundUpdateRequest updateRound)
    {
        try
        {
            var result = await _roundService.UpdateRound(updateRound);
            if (!result) return NotFound(new { Success = false, Message = "Không tìm thấy vòng thi" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa vòng thi thành công"
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

    #region Delete Round

    [HttpPatch]
    public async Task<IActionResult> DeleteRound(Guid id)
    {
        try
        {
            var result = await _roundService.DeleteRound(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa vòng thi thành công"
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

    #region Get Topic

    [HttpGet("gettopic/{id}")]
    public async Task<IActionResult> GetTopicInRound([FromRoute] Guid id, [FromQuery] ListModels listTopicmodel)
    {
        try
        {
            var (list, totalPage) = await _roundService.GetTopicInRound(id, listTopicmodel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đề trong vòng thi thành công",
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
                    List = new List<Round>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Round By EducationalLevel Id

    [HttpGet("getroundbyeducationallevelid/{id}")]
    public async Task<IActionResult> GetRoundByEducationalLevelId([FromQuery] ListModels listRoundModel,
        [FromRoute] Guid id)
    {
        try
        {
            var (list, totalPage) = await _roundService.GetRoundByEducationalLevelId(listRoundModel, id);
            if (totalPage < listRoundModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách vòng thi theo đối tượng thành công",
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
                    List = new List<Round>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get List Round

    [HttpGet("roundsforstaff")]
    public async Task<IActionResult> GetListRoundsForStaff()
    {
        try
        {
            var result = await _roundService.GetListRoundForCompetitor();
            if (!result.Any()) return NotFound(new { Success = false, Message = "Không tìm thấy vòng thi" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách các vòng thi thành công",
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