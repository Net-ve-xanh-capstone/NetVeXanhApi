using Application.BaseModels;
using Application.IService;
using Application.SendModels.Painting;
using Application.SendModels.Topic;
using Application.Services;
using Domain.Models;
using Infracstructures.SendModels.Painting;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

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
    public async Task<IActionResult> DraftPaintingForPreliminaryRound(CompetitorCreatePaintingRequest paintingrequest)
    {
        try
        {
            var result = await _paintingService.DraftPaintingForPreliminaryRound(paintingrequest);
            if (!result) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Draft Painting Success",
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

    #region Submit Painting For Preliminary Round

    [HttpPost("submitpainting1stround")]
    public async Task<IActionResult> SubmitPaintingForPreliminaryRound(CompetitorCreatePaintingRequest paintingRequest)
    {
        try
        {
            var result = await _paintingService.SubmitPaintingForPreliminaryRound(paintingRequest);
            if (!result) return NotFound(new BaseFailedResponseModel
            {
                Status = NotFound().StatusCode,
                Message = "Painting not found",
                Result = false
            });

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Submit Painting Success",
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

    #region Staff Submit Painting For Preliminary Round

    [HttpPost("submitepainting1stroundforCompetitor")]
    public async Task<IActionResult> SubmitPaintingForPreliminaryRoundForCompetitor(
        StaffCreatePaintingRequest staffCreatePainting)
    {
        try
        {
            var result = await _paintingService.StaffSubmitPaintingForPreliminaryRound(staffCreatePainting);
            if (!result) return NotFound(new { Success = false, Message = "Painting not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Submit Painting Success",
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
                Errors = ex,
                Result = false
            });
        }
    }

    #endregion

    #region Staff Submit Painting For Final Round

    [HttpPost("createpaintingfinalround")]
    public async Task<IActionResult> CreatePaintingForFinalRound(StaffCreatePaintingFinalRoundRequest request)
    {
        try
        {
            //Chưa validate
            var result = await _paintingService.StaffSubmitPaintingForFinalRound(request);
            if (!result) return NotFound(new { Success = false, Message = "Painting not found" });

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
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Painting

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePainting(UpdatePaintingRequest updatePainting)
    {
        try
        {
            var result = await _paintingService.UpdatePainting(updatePainting);
            if (!result) return NotFound();
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
    
    #region Update Painting

    [HttpPut("satffupdate")]
    public async Task<IActionResult> UpdatePaintingstaffpermisson(StaffUpdatePaintingRequest updatePaintingViewModel)
    {
        try
        {
            var result = await _paintingService.UpdatePaintingStaffPermission(updatePaintingViewModel);
            if (!result) return NotFound();
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

    #region Delete Painting

    [HttpPatch("deletepainting")]
    public async Task<IActionResult> DeletePainting(Guid id)
    {
        try
        {
            var result = await _paintingService.DeletePainting(id);
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

    #region Review Decision of Painting

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

    #region Final Decision of Painting

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

    #region Get Painting By Code

    [HttpGet("code")]
    public async Task<IActionResult> GetPaintingByCode([FromRoute] string code)
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
    public async Task<IActionResult> GetPaintingById([FromRoute] Guid id)
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

    #region Tracking Painting By Id

    [HttpGet("tracking/{id}")]
    public async Task<IActionResult> PaintingTracking([FromRoute] Guid id)
    {
        try
        {
            var result = await _paintingService.PaintingTracking(id);
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
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
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
    public async Task<IActionResult> ListPaintingByAccountId([FromQuery] ListModels listPaintingModel,
        [FromRoute] Guid id)
    {
        try
        {
            var (list, totalPage) = await _paintingService.ListPaintingByAccountId(id, listPaintingModel);
            if (totalPage < listPaintingModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
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

    [HttpPost("filterpainting")]
    public async Task<IActionResult> ListPaintingByAccountId(FilterPaintingRequest filterPainting,
        [FromQuery] ListModels listPaintingModel)
    {
        try
        {
            var (list, totalPage) = await _paintingService.FilterPainting(filterPainting, listPaintingModel);
            if (totalPage < listPaintingModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
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

    #region Get Painting By Account Contest
    [HttpGet("getpaintingbyaccountcontest")]
    public async Task<IActionResult> GetPaintingByAccountContest([FromQuery] Guid contestId, [FromQuery] Guid accountId)
    {
        try
        {
            var result = await _paintingService.GetPaintingByAccountContest(contestId, accountId);
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
}