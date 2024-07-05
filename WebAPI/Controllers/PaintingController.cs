﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Painting;
using Application.ViewModels.PaintingViewModels;
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
    public async Task<IActionResult> DraftPaintingForPreliminaryRound(Application.SendModels.Painting.PaintingRequest painting)
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Draft Painting Fail",
                Errors = ex
            });
        }
    }

    #endregion

    #region Submit Painting For Preliminary Round

    [HttpPost("submitepainting1stround")]
    public async Task<IActionResult> SubmitPaintingForPreliminaryRound(Application.SendModels.Painting.PaintingRequest painting)
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Submit Painting Fail",
                Errors = ex
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Create Painting For Final Round Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Update Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Delete Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Review Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Delete Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Get Painting Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Get Painting Fail",
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
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Get Painting Fail",
                Errors = ex
            });
        }
    }

    #endregion

    #region List 20 Wining Painting

    [HttpGet("list20")]
    public async Task<IActionResult> List20WiningPainting()
    {
        try
        {
            var result = await _paintingService.List20WiningPainting();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Painting Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Get Painting Fail",
                Errors = ex
            });
        }
    }

    #endregion
}