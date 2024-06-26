using Application.BaseModels;
using Application.IRepositories;
using Application.IService;
using Application.ViewModels.ContestViewModels;
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
    public async Task<IActionResult> CreateContest(AddContestViewModel contest)
    {
        try
        {
            var result = await _contestService.AddContest(contest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Award Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Contest

    [HttpPut]
    public async Task<IActionResult> UpdateContest(UpdateContestViewModel updateContestViewModel)
    {
        var result = await _contestService.UpdateContest(updateContestViewModel);
        if (result == null) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Update Successfully"
        });
    }

    #endregion

    #region Delete Contest

    [HttpPatch]
    public async Task<IActionResult> DeleteContest(Guid id)
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

    #endregion

    #region Get Contest By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContestById(Guid contestId)
    {
        try
        {
            var result = await _contestService.GetContestById(contestId);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Inventory Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get 5 recent contest year

    [HttpGet]
    public async Task<IActionResult> Get5RecentContestYear()
    {
        try
        {
            var result = await _contestService.Get5RecentYear();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get 5 Recent Contest Year Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion
}