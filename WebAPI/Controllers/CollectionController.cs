﻿using Application.IService;
using Application.ResponseModels;
using Application.ViewModels.CollectionViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/collection/")]
public class CollectionController : Controller
{
    private readonly ICollectionService _collectionService;

    public CollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }


    #region Create Collection

    [HttpPost]
    public async Task<IActionResult> CreateCollection(AddCollectionViewModel collection)
    {
        try
        {
            var result = await _collectionService.AddCollection(collection);
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

    #region Update Collection

    [HttpPut]
    public async Task<IActionResult> UpdateCollection(UpdateCollectionViewModel updateCollectionViewModel)
    {
        var result = await _collectionService.UpdateCollection(updateCollectionViewModel);
        if (result == null) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Update Successfully"
        });
    }

    #endregion

    #region Delete Collection

    [HttpPatch]
    public async Task<IActionResult> DeleteCollection(Guid id)
    {
        var result = await _collectionService.DeleteCollection(id);
        if (result == null) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Delete Successfully"
        });
    }

    #endregion

    #region Get Collection By Id

    [HttpGet]
    public async Task<IActionResult> GetCollectionById(Guid awardId)
    {
        try
        {
            var result = await _collectionService.GetCollectionById(awardId);
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
}