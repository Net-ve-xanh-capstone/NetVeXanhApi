using Application.BaseModels;
using Application.IRepositories;
using Application.IService;
using Application.SendModels.Contest;
using Application.Services;
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
            var validationResult = await _contestService.ValidateContestRequest(contest);
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
            var result = await _contestService.AddContest(contest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Contest Success",
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

    #region Update Contest

    [HttpPut]
    public async Task<IActionResult> UpdateContest(UpdateContest updateContest)
    {
        try
        {
            var validationResult = await _contestService.ValidateContestUpdateRequest(updateContest);
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
            var result = await _contestService.UpdateContest(updateContest);
            if (result == null) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Update Successfully"
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
            if (result == null) return NotFound();
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
                Message = "Get Contest Success",
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
                Message = "Get Contest Success",
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
                Message = "Get Contest Success",
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
            var result = await _contestService.GetAccountWithAwardPainting();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get 5 Recent Contest Year Success",
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
                Message = "Get Success",
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
                Message = "Get Contest Success",
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