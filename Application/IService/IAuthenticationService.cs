using Application.SendModels.Authentication;
using Application.ViewModels.AuthenticationViewModels;

namespace Application.IService;

public interface IAuthenticationService
{
    public Task<LoginResponse> Login(LoginRequest accountLogin);
    public Task<RegisterResponse> CreateCompetitor(CreateAccountRequest account);
    public Task<RegisterResponse> AdminCreateAccount(CreateAccountV2Request account);
    public Task<string> ReGenerateJwtToken(RefreshTokenRequest refreshToken);
    public Task<bool?> VerifyEmail(Guid id);
}