using Application.BaseModels;
using Application.IService;
using Application.SendModels.Authentication;
using Application.ViewModels.AuthenticationViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/authentications/")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    #region Login

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid input data. " + string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                RefreshToken = null,
                JwtToken = ""
            };
        var result = await _authenticationService.Login(request);
        return result;
    }

    #endregion

    #region Create Account

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateAccountRequest account)
    {
        try
        {
            var result = await _authenticationService.CreateCompetitor(account);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo tài khoản thành công",
                Result = result
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

    #region Create Account
    [Authorize(Roles = "Admin,Staff")]
    [HttpPost("registerforstaffandexaminer")]
    [SwaggerOperation(Tags = new[] { "Admin" })]
    public async Task<ActionResult<RegisterResponse>> CreateAccountV2(CreateAccountV2Request account)
    {
        try
        {
            var result = await _authenticationService.AdminCreateAccount(account);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo tài khoản thành công",
                Result = result
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

    #region Active Account
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpGet("verify/{id}")]
    public async Task<ActionResult> VerifyAccount(Guid id)
    {
        var result = await _authenticationService.VerifyEmail(id);
        if (result == false) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Successfully"
        });
    }

    #endregion

    #region ReGenerateJwtToken
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPost("/regeneratejwttoken")]
    public async Task<ActionResult<string>> ReGenerateJwtToken(RefreshTokenRequest token)
    {
        if (!ModelState.IsValid)
            return Unauthorized("Invalid input data. " + string.Join("; ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
        if (token.Expried < DateTime.Now) return Unauthorized("Token Expried");

        var result = await _authenticationService.ReGenerateJwtToken(token);
        if (result == "") return Unauthorized("Invaild Refresh Token");
        return result;
    }

    #endregion
    
    #region ResetPass
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [SwaggerOperation(Tags = new[] { "Authentication" })]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest userName)
    {
        if (string.IsNullOrWhiteSpace(userName.UsernName))
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Bắt buộc phải nhập tên đăng nhập.",
                Result = false
            });
        }

        var result = await _authenticationService.ForgotPassword(userName.UsernName);
    
        if (!result)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Tên đăng nhập không tồn tại trong hệ thống",
                Result = false
            });
        }

        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Message = "Mật khẩu đã được gửi tới địa chỉ email. Vui lòng check email.",
            Result = true
        });
    }

    #endregion

}