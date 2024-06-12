﻿using Application.IService;
using Application.ResponseModels;
using Infracstructures.SendModels.BaseModels;
using Infracstructures.SendModels.Sponsor;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
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
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new { Success = false, Message = "Invalid input data. " + errorMessages });
            }
            var result = await _sponsorService.CreateSponsor(sponsor);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create sponsor Success",
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
    
    #region Get All sponsor

    [HttpGet]
    public async Task<IActionResult> GetAllsponsor(ListModels listModel)
    {
        try
        {
            var (list, totalPage) = await _sponsorService.GetListSponsor(listModel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Inventory Success",
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
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Sponsor By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetsponsorById(Guid id)
    {
        try
        {
            var result = await _sponsorService.GetSponsorById(id);
            if (result == null)
            {
                return NotFound(new { Success = false, Message = "Sponsor not found" });
            }
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
    
    #region Update sponsor

    [HttpPut]
    public async Task<IActionResult> UpdateSponsor(SponsorUpdateRequest updatesponsor)
    {
        var result = await _sponsorService.UpdateSponsor(updatesponsor);
        if (result == null) return NotFound(new { Success = false, Message = "Sponsor not found" });
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Update Successfully"
        });
    }

    #endregion

    #region Delete sponsor

    [HttpPatch]
    public async Task<IActionResult> Deletesponsor(Guid id)
    {
        var result = await _sponsorService.DeleteSponsor(id);
        if (result == null) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Delete Successfully"
        });
    }

    #endregion


}