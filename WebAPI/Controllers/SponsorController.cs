using Application.BaseModels;
using Application.IService;
using Application.SendModels.Topic;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Infracstructures.SendModels.Sponsor;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/sponsors/")]
public class SponsorController : Controller
{
    private readonly ISponsorService _sponsorService;

    public SponsorController(ISponsorService sponsorService)
    {
        _sponsorService = sponsorService;
    }


    #region Create sponsor

    [HttpPost]
    public async Task<IActionResult> CreateSponsor(SponsorRequest sponsor)
    {
        try
        {
            var result = await _sponsorService.CreateSponsor(sponsor);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create sponsor Success",
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

    #region Get sponsor By Page

    [HttpGet]
    public async Task<IActionResult> GetSponsorByPage([FromQuery] ListModels listSponsorModel)
    {
        try
        {
            var (list, totalPage) = await _sponsorService.GetListSponsor(listSponsorModel);
            if (totalPage < listSponsorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Sponsor Success",
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
                    List = new List<Sponsor>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get sponsor By Page

    [HttpGet("getallsponsor")]
    public async Task<IActionResult> GetAllSponsor()
    {
        try
        {
            var result = await _sponsorService.GetAllSponsor();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Sponsor Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Sponsor>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Sponsor By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetsponsorById([FromRoute] Guid id)
    {
        try
        {
            var result = await _sponsorService.GetSponsorById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Sponsor not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Sponsor Success",
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

    #region Update sponsor

    [HttpPut]
    public async Task<IActionResult> UpdateSponsor(SponsorUpdateRequest updatesponsor)
    {
        try
        {
            var result = await _sponsorService.UpdateSponsor(updatesponsor);
            if (!result) return NotFound(new { Success = false, Message = "Sponsor not found" });
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

    #region Delete sponsor

    [HttpPatch]
    public async Task<IActionResult> Deletesponsor(Guid id)
    {
        try
        {
            var result = await _sponsorService.DeleteSponsor(id);
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