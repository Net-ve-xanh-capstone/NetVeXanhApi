namespace Application.SendModels.Authentication;

public class CreateAccountRequest
{
    public string? Username { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public string Password { get; set; }

    public string? Phone { get; set; }

    public bool Gender { get; set; } = true;

    public DateTime Birthday { get; set; }
}