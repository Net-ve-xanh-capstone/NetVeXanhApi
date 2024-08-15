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
[Route("api/awards/")]
public class AwardController : Controller
{
    private readonly IAwardService _awardService;

    public AwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    #region Create Award
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createAward"> Rank = FirstPrize |  SecondPrize | ConsolationPrize | Preliminary | OtherAward |</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAward(CreateAwardSendModel createAward)
    {
        try
        {
            var result = await _awardService.AddAward(createAward);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo Giải mới thành công",
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
                Message = "Chỉnh sửa Giải thành công"
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
                Message = "Xóa Giải thành công"
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
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách giải thành công",
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
    
    #region Get List Award By Round Id
    /// <summary>
    /// Lấy giải theo vòng để nhập vào số lượng của giải đó để tạo lịch chấm 
    /// </summary>
    /// <param name="roundId"></param>
    /// <returns></returns>
    [HttpGet("Round/{roundId}")]
    public async Task<IActionResult> GetAllAward(Guid roundId)
    {
        try
        {
            var result = await _awardService.GetAwardsByRoundId(roundId);
            if (result == null) return NotFound(new { Success = false, Message = "Không tìm thấy giải" });
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
                Result = null,
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
                Message = "Lấy chi tiết giải thưởng thành công.",
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