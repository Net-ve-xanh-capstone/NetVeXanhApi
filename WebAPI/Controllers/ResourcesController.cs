using System.Resources;
using Application.BaseModels;
using Application.IService;
using Application.SendModels.Resources;
using Application.SendModels.Topic;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/resources/")]
public class ResourcesController : Controller
{
    private readonly IResourcesService _resourcesService;

    public ResourcesController(IResourcesService resourcesService)
    {
        _resourcesService = resourcesService;
    }

    #region Create Resources

    [HttpPost]
    public async Task<IActionResult> CreateResources(ResourcesRequest resources)
    {
        try
        {
            var validationResult = await _resourcesService.ValidateResourceRequest(resources);
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
            var result = await _resourcesService.CreateResources(resources);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Resources Success",
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

    #region Get Resources

    [HttpGet("getallresource")]
    public async Task<IActionResult> GetResourcesByPage()
    {
        try
        {
            var result = await _resourcesService.GetListResources();
            
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Resources Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = new List<Resources>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Resources By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetResourcesById(Guid id)
    {
        try
        {
            var result = await _resourcesService.GetResourcesById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Resources not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Resources Success",
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

    #region Update Resources

    [HttpPut]
    public async Task<IActionResult> UpdateResources(ResourcesUpdateRequest updateResources)
    {
        try
        {
            var validationResult = await _resourcesService.ValidateResourceUpdateRequest(updateResources);
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
            var result = await _resourcesService.UpdateResources(updateResources);
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

    #region Delete Resources

    [HttpPatch]
    public async Task<IActionResult> DeleteResources(Guid id)
    {
        try
        {
            var result = await _resourcesService.DeleteResources(id);
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