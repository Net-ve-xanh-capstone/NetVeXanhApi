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

    [HttpPost]
    public async Task<IActionResult> CreateAward(AwardRequest award)
    {
        try
        {
            if (!Enum.IsDefined(typeof(RankAward), award.Rank))
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Giải không tồn tại. Vui lòng kiểm tra lại!"
                });
            var result = await _awardService.AddAward(award);
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
    
    #region Get List Award By Contest Id
    [HttpGet("contest/{contestId}")]
    public async Task<IActionResult> GetAllAward(Guid contestId)
    {
        try
        {
            var result = await _awardService.GetAwardsByContestId(contestId);
            if (result == null) return NotFound(new { Success = false, Message = "Topic not found" });
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
                Message = "Trang vượt quá số lượng trang cho phép.",
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