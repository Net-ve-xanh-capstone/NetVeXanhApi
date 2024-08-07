﻿using Application.BaseModels;
using Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/address/")]
public class AddressController : ControllerBase
{
    private readonly IDistrictService _districtService;

    public AddressController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    #region Get All Award

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var district = await _districtService.GetAll();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách Quận/ Huyện thành công",
                Result = district
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Award

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var district = await _districtService.GetById(id);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách Phường thành công",
                Result = district
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Errors = ex
            });
        }
    }

    #endregion
}