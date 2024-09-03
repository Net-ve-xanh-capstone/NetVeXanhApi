using System.Security.Cryptography;
using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Authentication;
using Application.SendModels.Painting;
using Application.ViewModels.AuthenticationViewModels;
using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Excel;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthentication _authentication;
    private readonly IClaimsService _claimsService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public AuthenticationService(IUnitOfWork unitOfWork, IAuthentication authentication, IMapper mapper,
        IMailService mailService, IClaimsService claimsService, IValidatorFactory validator)
    {
        _claimsService = claimsService;
        _mailService = mailService;
        _authentication = authentication;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validator;
    }


    #region Login

    public async Task<LoginResponse> Login(LoginRequest accountLogin)
    {
        var response = new LoginResponse();
        var account = await _unitOfWork.AccountRepo.FindUserByUsername(accountLogin.Username);
        //check null
        if (account != null)
        {
            //Verify Password
            var check = _authentication.VerifyPassword(account.Password, accountLogin.Password);
            if (check is true)
            {
                response.Success = true;
                response.Message = "Đăng nhập thành công";
                response.JwtToken =
                    _authentication.GenerateToken(account);
                _unitOfWork.AccountRepo.Update(account);
                await _unitOfWork.SaveChangesAsync();

                return response;
            }

            response.Success = false;
            response.Message = "Đăng nhập không thành công.";
            return response;
        }

        response.Success = false;
        response.Message = "Đăng nhập không thành công.";
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
            _ => throw new ArgumentException("Vai trờ không hợp lệ")
        };

        var number = await _unitOfWork.AccountRepo.CreateNumberOfAccountCode(prefix);
        return $"{prefix}-{number:D6}";
    }

    #endregion

    #region Forgot Password

    public async Task<bool> ForgotPassword(string userName)
    {
        var account = await _unitOfWork.AccountRepo.FindUserByUsername(userName);
        if (account != null)
        {
            var newPass = RandomPassword();
            account!.Password = _authentication.Hash(newPass);
            await _unitOfWork.SaveChangesAsync();
            MailModel mail = new MailModel();
            mail.To = account!.Email!;
            mail.Body = newPass;
            mail.Subject = "New passWord";
            await _mailService.SendEmail(mail);
            return true;
        }
        return false;
    }
    public string RandomPassword()
    {
        var random = new Random();
        var length = 16; // Length of the random string
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        var randomString = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return randomString;
    }

    #endregion


    #region Create Account

    public async Task<RegisterResponse> CreateCompetitor(CreateAccountRequest createAccount)
    {
        var validationResult = await ValidateCreateAccountRequest(createAccount);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var response = new RegisterResponse();

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

        response.Message = "Tạo mới thành công.";
        response.Success = true;
        
        await _mailService.SendConfirmRegistration(account);

        return response;
    }

    public async Task<RegisterResponse> AdminCreateAccount(CreateAccountV2Request createAccount)
    {
        var response = new RegisterResponse();
        if (createAccount.Role != Role.Examiner.ToString() && createAccount.Role != Role.Staff.ToString())
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

        var account = _mapper.Map<Account>(createAccount);
        //if not exist
        var password = RandomPassword();
        account.Password = _authentication.Hash(password);
        account.Status = AccountStatus.Active.ToString();

        //Generate Code
        account.Code = await GenerateAccountCode((Role)Enum.Parse(typeof(Role), account.Role));
        account.Username = account.Code;

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

        await _mailService.SendAccountInformation(account, password);
        return response;
    }

    #endregion

    public async Task<ValidationResult> ValidateCreateAccountRequest(CreateAccountRequest painting)
    {
        return await _validatorFactory.CreateAccountRequestValidator.ValidateAsync(painting);
    }
}