﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Report;
using Application.Services;
using Application.ViewModels.AwardViewModels;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/reports/")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    #region Create Report

    [HttpPost]
    public async Task<IActionResult> CreateReport(ReportRequest report)
    {
        try
        {
            var result = await _reportService.AddReport(report);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Report Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Create Report Fail",
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Report

    [HttpPut]
    public async Task<IActionResult> UpdateReport(UpdateReportRequest updateReport)
    {
        try
        {
            var result = await _reportService.UpdateReport(updateReport);
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
                Message = "Update Fail",
                Errors = ex
            });
        }

    }

    #endregion

    #region Delete Report

    [HttpPatch]
    public async Task<IActionResult> DeleteReport(Guid id)
    {
        try
        {
            var result = await _reportService.DeleteReport(id);
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
                Message = "Delete Fail",
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Report

    [HttpGet]
    public async Task<IActionResult> GetAllReport([FromQuery] ListModels listReportModel)
    {
        try
        {
            var (list, totalPage) = await _reportService.GetAllReportPending(listReportModel);
            if (totalPage < listReportModel.PageNumber)
            {
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            }
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Report Success",
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
                Message = "Get Report Fail",
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Report By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportById([FromRoute]Guid id)
    {
        try
        {
            var result = await _reportService.GetReportById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Report Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Get Report Fail",
                Errors = ex
            });
        }
    }

    #endregion
}
