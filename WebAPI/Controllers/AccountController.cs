using Application.BaseModels;
using Application.IService;
using Application.SendModels.AccountSendModels;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/accounts/")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    #region Get All 

    /// <summary>
    ///     Lấy danh sách tất cả tài khoản
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("getallaccount")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllAccount()
    {
        try
        {
            var result = await _accountService.GetAllAccount();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách tài khoản thành công",
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

    #region Get All Competitor

    /// <summary>
    ///     Lấy danh sách người dự thi có phân trang
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Staff")]
    [HttpGet("getallcompetitorwithpagination")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllCompetitorWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListCompetitor(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách người dự thi thành công",
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

    /// <summary>
    ///     Lấy danh sách giám khảo có phân trang
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Staff")]
    [HttpGet("getallexaminerwithpagination")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllExaminerWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListExaminer(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách giám khảo thành công",
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

    /// <summary>
    ///     Lấy danh sách staff có phân trang
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("getallstaffwithpagination")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllStaffWithPagination([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListStaff(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách nhân viên thành công",
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
    [Authorize(Roles = "Admin, Staff")]
    [HttpGet("getallcompetitor")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllCompetitor()
    {
        try
        {
            var result = await _accountService.GetAllCompetitor();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách thí sinh thành công",
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
    [Authorize(Roles = "Admin, Staff")]
    [HttpGet("getallexaminer")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllExaminer()
    {
        try
        {
            var result = await _accountService.GetAllExaminer();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách giám khảo thành công",
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
    [Authorize(Roles = "Admin")]
    [HttpGet("getallstaff")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var result = await _accountService.GetAllStaff();

            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách nhân viên thành công",
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
    [Authorize(Roles = "Admin, Staff")]
    [HttpGet("getallinactiveaccountwithpagination")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAllInactiveAccount([FromQuery] ListModels listCompetitorModel)
    {
        try
        {
            var (list, totalPage) = await _accountService.GetListInactiveAccount(listCompetitorModel);
            if (totalPage < listCompetitorModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách tài khoản bị khóa thành công",
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
    [Authorize(Roles = "Admin")]
    [HttpGet("getaccountbyid/{id}")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        try
        {
            var result = await _accountService.GetAccountById(id);
            if (result == null)
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Tài khoản không tồn tại"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin tài khoản thành công",
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
                    Message = "Tài khoản không tồn tại"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin người dự thi thành công",
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
                    Message = "Tài khoản không tồn tại"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy thông tin tài khoản thành công",
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
                Message = "Lấy thông tin tài khoản có giải trong 3 năm thành công"
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

    #region Update Account

    [HttpPut]
    public async Task<IActionResult> UpdateAccount(AccountUpdateRequest updateAccount)
    {
        try
        {
            var result = await _accountService.UpdateAccount(updateAccount);
            if (result == null) return NotFound(new { Success = false, Message = "Tài khoản không tồn tại" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa thông tin tài khoản thành công"
            });
        }
        catch (ValidationException ex)
        {
            var firstErrorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage;
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = firstErrorMessage,
                Result = false
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
    [Authorize(Roles = "Admin")]
    [HttpPatch("inactiveaccount")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> InactiveAccount(Guid id)
    {
        try
        {
            var result = await _accountService.InactiveAccount(id);
            if (result == null) return NotFound(new { Success = false, Message = "Tài khoản không tồn tại" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Khóa tài khoản thành công"
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
    [Authorize(Roles = "Admin")]
    [HttpPatch("activeaccount")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<IActionResult> ActiveAccount(Guid id)
    {
        try
        {
            var result = await _accountService.ActiveAccount(id);
            if (result == null) return NotFound(new { Success = false, Message = "Tài khoản không tồn tại" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Mở khóa tài khoản thành công"
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