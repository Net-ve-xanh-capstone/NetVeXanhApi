using Application.BaseModels;
using Application.IService;
using Application.SendModels.AccountSendModels;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/accounts/")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        
        _accountService = accountService;
    }

    #region Get All Competitor

    [HttpGet("getallcompetitorwithpagination")]
    public async Task<IActionResult> GetAllCompetitorWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListCompetitor(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
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
                    List = new List<Account>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Examiner

    [HttpGet("getallexaminerwithpagination")]
    public async Task<IActionResult> GetAllExaminerWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListExaminer(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
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
                    List = new List<Account>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region get all staff

    [HttpGet("getallstaffwithpagination")]
    public async Task<IActionResult> GetAllStaffWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListStaff(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
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
                    List = new List<Account>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Competitor

    [HttpGet("getallcompetitor")]
    public async Task<IActionResult> GetAllCompetitor()
    {
        try
        {
            var result = await _accountService.GetAllCompetitor();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Account>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Examiner

    [HttpGet("getallexaminer")]
    public async Task<IActionResult> GetAllExaminer()
    {
        try
        {
            var result = await _accountService.GetAllExaminer();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Account>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region get all staff

    [HttpGet("getallstaff")]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var result = await _accountService.GetAllStaff();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Account>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Inactive Account

    [HttpGet("getallinactiveaccountwithpagination")]
    public async Task<IActionResult> GetAllInactiveAccount([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListInactiveAccount(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
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
                    List = new List<Account>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Account By Id

    [HttpGet("getaccountbyid/{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        try
        {
            var result = await _accountService.GetAccountById(id);
            if (result == null)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Account Dont Exist"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Competitor By Id

    [HttpGet("getcompetitorbyid/{id}")]
    public async Task<IActionResult> GetCompetitorById(Guid id)
    {
        try
        {
            var result = await _accountService.GetCompetitorById(id);
            if (result == null)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Account Dont Exist"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Account By Code

    [HttpGet("getaccountbycode/{code}")]
    public async Task<IActionResult> GetAccountByCode(string code)
    {
        try
        {
            var result = await _accountService.GetAccountByCode(code);
            if (result == null)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Account Dont Exist"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Account Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Account

    [HttpPut]
    public async Task<IActionResult> UpdateAccount(AccountUpdateRequest updateAccount)
    {
        try
        {
            var result = await _accountService.UpdateAccount(updateAccount);
            if (result == null) return NotFound(new { Success = false, Message = "Account not found" });
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
                Message = "Validation failed",
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

    #region Inactive Account

    [HttpPatch("inactiveaccount")]
    public async Task<IActionResult> InactiveAccount(Guid id)
    {
        try
        {
            var result = await _accountService.InactiveAccount(id);
            if (result == null) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Inactive Successfully"
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

    #region Active Account

    [HttpPatch("activeaccount")]
    public async Task<IActionResult> ActiveAccount(Guid id)
    {
        try
        {
            var result = await _accountService.ActiveAccount(id);
            if (result == null) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Active Successfully"
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

    #region ListAccountHaveAwardIn3NearestContest

    [HttpGet("getlistwinnerin3nearestcontest")]
    public async Task<IActionResult> ListAccountHaveAwardIn3NearestContest()
    {
        try
        {
            var result = await _accountService.ListAccountHaveAwardIn3NearestContest();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Get List Account Success"
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Account>(),
                Errors = ex
            });
        }
    }

    #endregion
}