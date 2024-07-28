using System.Security.Cryptography;
using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Authentication;
using Application.ViewModels.AuthenticationViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthentication _authentication;
    private readonly IClaimsService _claimsService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IUnitOfWork unitOfWork, IAuthentication authentication, IMapper mapper,
        IMailService mailService, IClaimsService claimsService)
    {
        _claimsService = claimsService;
        _mailService = mailService;
        _authentication = authentication;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    #region Login

    public async Task<LoginResponse> Login(LoginRequest accountLogin)
    {
        var response = new LoginResponse();
        var account = await _unitOfWork.AccountRepo.Login(accountLogin.Username);
        //check null
        if (account != null)
        {
            //Verify Password
            var check = _authentication.Verify(account.Password, accountLogin.Password);
            if (check is true)
            {
                response.Success = true;
                response.Message = "Đăng nhập thành công";
                response.JwtToken =
                    _authentication.GenerateToken(account);
                response.RefreshToken = new RefreshToken();
                response.RefreshToken.Token = RefreshToken();

                //add refresh token to DB
                account.RefreshToken = response.RefreshToken.Token;
                _unitOfWork.AccountRepo.Update(account);
                await _unitOfWork.SaveChangesAsync();

                return response;
            }

            response.Success = false;
            response.Message = "Sai mật khẩu";
            return response;
        }

        response.Success = false;
        response.Message = "Tên đăng nhập không đúng";
        return response;
    }

    #endregion

    #region Create Account

    public async Task<RegisterResponse> CreateAccount(CreateAccountRequest createAccount)
    {
        var response = new RegisterResponse();
        if (!Enum.IsDefined(typeof(Role), createAccount.Role))
        {
            response.Message = "!";
            response.Success = false;
            return response;
        }

        if (await _unitOfWork.AccountRepo.CheckDuplicateEmail(createAccount.Email))
        {
            response.Message = "Email đã có tài khoản sử dụng!";
            response.Success = false;
            return response;
        }

        if (await _unitOfWork.AccountRepo.CheckDuplicatePhone(createAccount.Phone))
        {
            response.Message = "Số điện thoại đã có tài khoản sử dụng!";
            response.Success = false;
            return response;
        }

        if (await _unitOfWork.AccountRepo.CheckDuplicateUsername(createAccount.Username))
        {
            response.Message = "Tên đăng nhập đã có tài khoản sử dụng!";
            response.Success = false;
            return response;
        }

        var account = _mapper.Map<Account>(createAccount);
        //if not exist
        account.Password = _authentication.Hash(createAccount.Password);
        account.Status = AccountStatus.Active.ToString();

        //Generate Code
        account.Code = await GenerateAccountCode((Role)Enum.Parse(typeof(Role), account.Role));

        await _unitOfWork.AccountRepo.AddAsync(account);
        var check = await _unitOfWork.SaveChangesAsync() > 0;

        if (check is false)
        {
            response.Message = "Tạo thất bại!";
            response.Success = true;
            return response;
        }

        response.Message = "Tạo thành công";
        response.Success = true;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "Active Account";
        mail.Body = $"Link ID {account.Id}";
        await _mailService.SendEmail(mail);

        return response;
    }

    #endregion

    #region ReGenerate JwtToken Account

    public async Task<string> ReGenerateJwtToken(RefreshTokenRequest refreshToken)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(refreshToken.Id);
        if (account != null)
            return _authentication.GenerateToken(account);
        return "";
    }

    #endregion

    #region Logout Account

    public async Task<bool> Logout(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(id);
        if (account != null)
        {
            account.RefreshToken = "";
            return true;
        }

        return false;
    }

    #endregion

    #region Verify Email

    public async Task<bool?> VerifyEmail(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(id);
        if (account == null) return false;
        account.Status = AccountStatus.Active.ToString();
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Refresh Token

    public string RefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    #endregion
    
    #region Generate Account Code

    private async Task<string> GenerateAccountCode(Role role)
    {
        var prefix = role switch
        {
            Role.Guardian => "GH",
            Role.Competitor => "TS",
            Role.Staff => "NV",
            Role.Admin => "AD",
            Role.Examiner => "GK",
            _ => throw new ArgumentException("Invalid role")
        };

        var number = await _unitOfWork.AccountRepo.CreateNumberOfAccountCode(prefix);
        return $"{prefix}-{number:D6}";
    }

    #endregion
}