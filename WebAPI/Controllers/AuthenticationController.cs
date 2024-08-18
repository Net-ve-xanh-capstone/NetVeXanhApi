using Application.BaseModels;
using Application.IService;
using Application.SendModels.Authentication;
using Application.ViewModels.AuthenticationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<RegisterResponse>> Register(CreateAccountRequest account)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new RegisterResponse
            {
                Success = false,
                Message = "Invalid input data. " + string.Join("; ", errorMessages),
                Data = ""
            };
        }

        return await _authenticationService.CreateCompetitor(account);
    }

    #endregion
    
    #region Create Account

    [AllowAnonymous]
    [HttpPost("registerforstaffandexaminer")]
    public async Task<ActionResult<RegisterResponse>> CreateAccountV2(CreateAccountV2Request account)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new RegisterResponse
            {
                Success = false,
                Message = "Invalid input data. " + string.Join("; ", errorMessages),
                Data = ""
            };
        }

        return await _authenticationService.AdminCreateAccount(account);
    }

    #endregion

    #region Active Account

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

    #region Logout Account

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult> LogoutAccount(Guid id)
    {
        var result = await _authenticationService.Logout(id);
        if (result == false) return NotFound();
        return Ok(new BaseResponseModel
        {
            Status = Ok().StatusCode,
            Result = result,
            Message = "Successfully"
        });
    }

    #endregion
}