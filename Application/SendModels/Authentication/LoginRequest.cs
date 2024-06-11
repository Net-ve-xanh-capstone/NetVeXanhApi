namespace Application.SendModels.Authentication;

public class LoginRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}