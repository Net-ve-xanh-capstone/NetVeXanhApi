﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ScheduleController : Controller
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }
    
        #region Create Schedule

    [HttpPost]
    public async Task<IActionResult> CreateSchedule(ScheduleRequest Schedule)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new { Success = false, Message = "Invalid input data. " + errorMessages });
            }

            var result = await _scheduleService.CreateSchedule(Schedule);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Schedule Success",
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

    #region Get Schedule By Page

    [HttpGet]
    public async Task<IActionResult> GetScheduleByPage([FromQuery] ListModels listModel)
    {
        try
        {
            var (list, totalPage) = await _scheduleService.GetListSchedule(listModel);
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

    #region Get Schedule By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleById(Guid id)
    {
        try
        {
            var result = await _scheduleService.GetScheduleById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
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

    #region Update Schedule

    [HttpPut]
    public async Task<IActionResult> UpdateSchedule(ScheduleUpdateRequest updateSchedule)
    {
        var result = await _scheduleService.UpdateSchedule(updateSchedule);
        if (result == null) return NotFound(new { Success = false, Message = "Schedule not found" });
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Update Successfully"
        });
    }

    #endregion

    #region Delete Schedule

    [HttpPatch]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var result = await _scheduleService
            .DeleteSchedule(id);
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