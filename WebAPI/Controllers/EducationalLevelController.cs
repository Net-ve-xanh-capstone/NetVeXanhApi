﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.EducationalLevel;
using Application.SendModels.Round;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/educationallevels/")]
public class EducationalLevelController : Controller
{
    private readonly IEducationalLevelService _educationalLevelService;

    public EducationalLevelController(IEducationalLevelService educationalLevelService)
    {
        _educationalLevelService = educationalLevelService;
    }

    #region Create EducationalLevel

    [HttpPost]
    public async Task<IActionResult> CreateEducationalLevel(EducationalLevelRequest EducationalLevel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new { Success = false, Message = "Invalid input data. " + errorMessages });
            }

            var result = await _educationalLevelService.CreateEducationalLevel(EducationalLevel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create EducationalLevel Success",
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

    #region Get EducationalLevel By Page

    [HttpGet]
    public async Task<IActionResult> GetEducationalLevelByPage([FromQuery] ListModels listModel)
    {
        try
        {
            var (list, totalPage) = await _educationalLevelService.GetListEducationalLevel(listModel);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
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

    #region Get EducationalLevel By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationalLevelById(Guid id)
    {
        try
        {
            var result = await _educationalLevelService.GetEducationalLevelById(id);
            if (result == null) return NotFound(new { Success = false, Message = "EducationalLevel not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get EducationalLevel Success",
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

    #region Update EducationalLevel

    [HttpPut]
    public async Task<IActionResult> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel)
    {
        var result = await _educationalLevelService.UpdateEducationalLevel(updateEducationalLevel);
        if (result == null) return NotFound(new { Success = false, Message = "EducationalLevel not found" });
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Update Successfully"
        });
    }

    #endregion

    #region Delete EducationalLevel

    [HttpPatch]
    public async Task<IActionResult> DeleteEducationalLevel(Guid id)
    {
        var result = await _educationalLevelService.DeleteEducationalLevel(id);
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