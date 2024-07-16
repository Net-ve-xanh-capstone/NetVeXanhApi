﻿using System.Collections.Generic;
using Application.BaseModels;
using Application.IService;
using Application.SendModels.Painting;
using Application.Services;
using Application.ViewModels.PaintingViewModels;
using Domain.Models;
using Infracstructures.SendModels.Painting;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/paintings/")]
public class PaintingController : Controller
{
    private readonly IPaintingService _paintingService;

    public PaintingController(IPaintingService paintingService)
    {
        _paintingService = paintingService;
    }


    #region Draft Painting For Preliminary Round

    [HttpPost("draftepainting1stround")]
    public async Task<IActionResult> DraftPaintingForPreliminaryRound(PaintingRequest2 painting)
    {
        try
        {
            var result = await _paintingService.DraftPaintingForPreliminaryRound(painting);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Draft Painting Success",
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

    #region Submit Painting For Preliminary Round

    [HttpPost("submitepainting1stround")]
    public async Task<IActionResult> SubmitPaintingForPreliminaryRound(PaintingRequest painting)
    {
        try
        {
            var result = await _paintingService.SubmitPaintingForPreliminaryRound(painting);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Submit Painting Success",
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

    #region Submit Painting For Preliminary Round For Competitor

    [HttpPost("submitepainting1stroundforCompetitor")]
    public async Task<IActionResult> SubmitPaintingForPreliminaryRoundForCompetitor(PaintingRequest2 painting)
    {
        try
        {
            var result = await _paintingService.SubmitPaintingForPreliminaryRoundForCompetitor(painting);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Submit Painting Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex,
                Result = false,
            });
        }
    }

    #endregion

    #region Create Painting For Final Round

    [HttpPost("createpaintingfinalround")]
    public async Task<IActionResult> CreatePaintingForFinalRound(Application.SendModels.Painting.PaintingRequest painting)
    {
        try
        {
            var result = await _paintingService.AddPaintingForFinalRound(painting);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Painting For Final Round Success",
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

    #region Update Painting

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePainting(UpdatePaintingRequest updatePaintingViewModel)
    {
        try
        {

            var result = await _paintingService.UpdatePainting(updatePaintingViewModel);
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

    #region Delete Painting

    [HttpPatch("deletepainting")]
    public async Task<IActionResult> DeletePainting(Guid id)
    {
        try
        {
            var result = await _paintingService.DeletePainting(id);
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
    
    /*#region Submitted Painting

    [HttpPost("submit")]
    public async Task<IActionResult> SubmittedPainting(Guid id)
    {
        var result = await _paintingService.SubmitPainting(id);
        if (result == null) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Delete Successfully"
        });
    }

    #endregion*/
    
    #region  Review Decision of Painting

    [HttpPatch("review")]
    public async Task<IActionResult> ReviewDecisionOfPainting(PaintingUpdateStatusRequest request)
    {
        try
        {
            var result = await _paintingService.ReviewDecisionOfPainting(request);
            if (result == null) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Review Successfully"
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
    
    #region  Final Decision of Painting

    [HttpPatch("finaldecision")]
    public async Task<IActionResult> FinalDecisionOfPainting(PaintingUpdateStatusRequest request)
    {
        try
        {
            var result = await _paintingService.FinalDecisionOfPainting(request);
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

    #region Get Painting By Code

    [HttpGet("code")]
    public async Task<IActionResult> GetPaintingByCode([FromRoute]string code)
    {
        try
        {
            var result = await _paintingService.GetPaintingByCode(code);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Painting Success",
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
    
    #region Get Painting By Id

    [HttpGet("{id}")]

    public async Task<IActionResult> GetPaintingById([FromRoute]Guid id)
    {
        try
        {
            var result = await _paintingService.GetPaintingById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Painting Success",
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

    #region Get All Painting

    [HttpGet("list")]
    public async Task<IActionResult> GetAllAward([FromQuery] ListModels listPaintingModel)
    {
        try
        {
            var (list, totalPage) = await _paintingService.GetListPainting(listPaintingModel);
            if (totalPage < listPaintingModel.PageNumber)
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
                Message = "Get Painting Success",
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
                    List = new List<Painting>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List 16 Wining Painting

    [HttpGet("list16winingpainting")]
    public async Task<IActionResult> List16WiningPainting()
    {
        try
        {
            var result = await _paintingService.List16WiningPainting();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Painting Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Painting>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region List Painting By Account Id

    [HttpGet("listpaintingbyaccountid/{id}")]
    public async Task<IActionResult> ListPaintingByAccountId([FromQuery] ListModels listPaintingModel, [FromRoute] Guid id)
    {
        try
        {
            var (list, totalPage) = await _paintingService.ListPaintingByAccountId(id, listPaintingModel);
            if (totalPage < listPaintingModel.PageNumber)
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
                Message = "Get Painting Success",
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
                    List = new List<Painting>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Filter Painting

    [HttpGet("filterpainting")]
    public async Task<IActionResult> ListPaintingByAccountId([FromQuery] FilterPaintingRequest filterPainting,[FromQuery] ListModels listPaintingModel)
    {
        try
        {
            var (list, totalPage) = await _paintingService.FilterPainting(filterPainting, listPaintingModel);
            if (totalPage < listPaintingModel.PageNumber)
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
                Message = "Filter Painting Success",
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
                    List = new List<Painting>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion


}