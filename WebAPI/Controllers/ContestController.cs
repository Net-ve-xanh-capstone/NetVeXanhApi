using Application.BaseModels;
using Application.IRepositories;
using Application.IService;
using Application.SendModels.Contest;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/contests/")]
public class ContestController : Controller
{
    private readonly IContestService _contestService;

    public ContestController(IContestService contestService, IContestRepository contestRepository)
    {
        _contestService = contestService;
    }

    #region Create Contest

    [HttpPost]
    public async Task<IActionResult> CreateContest(ContestRequest contest)
    {
        try
        {
            var result = await _contestService.AddContest(contest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo cuộc thi thành công",
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

    #region Update Contest

    [HttpPut]
    public async Task<IActionResult> UpdateContest(UpdateContest updateContest)
    {
        try
        {
            var result = await _contestService.UpdateContest(updateContest);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa cuộc thi thành công"
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

    #region Delete Contest

    [HttpPatch]
    public async Task<IActionResult> DeleteContest(Guid id)
    {
        try
        {
            var result = await _contestService.DeleteContest(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa cuộc thi thành công"
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

    #region Get Contest By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContestById([FromRoute] Guid id)
    {
        try
        {
            var result = await _contestService.GetContestById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin cuộc thi thành công",
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

    #region Get All Contest

    [HttpGet("getallcontest")]
    public async Task<IActionResult> GetAllContest()
    {
        try
        {
            var result = await _contestService.GetAllContest();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách cuộc thi thành công",
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

    #region get contest for filter painting

    [HttpGet("getcontestforfilterpainting")]
    public async Task<IActionResult> GetContestForFilterPainting()
    {
        try
        {
            var result = await _contestService.GetContestForFilterPainting();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách cuộc thi thành công",
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

    #region Get 5 recent contest year

    [HttpGet("get5recentyear")]
    public async Task<IActionResult> Get5RecentContestYear()
    {
        try
        {
            var result = await _contestService.Get5RecentYear();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy 5 năm của 5 cuộc thi gần nhất thành công",
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

    #region Get 5 recent contest year

    [HttpGet("getaccountwithawardpainting")]
    public async Task<IActionResult> GetAccountWithAwardPainting()
    {
        try
        {
            var result = await _contestService.GetAccountWithAwardPainting();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin các tài khoản được giải trong 5 cuộc thi gần nhất thành công",
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

    #region Get Nearest Contest

    [HttpGet("getnearestcontest")]
    public async Task<IActionResult> GetNearestContest()
    {
        try
        {
            var result = await _contestService.GetNearestContest();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin cuộc thi gần nhất thành công",
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