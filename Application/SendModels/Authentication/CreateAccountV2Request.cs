namespace Application.SendModels.Authentication;

public class CreateAccountV2Request
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Phone { get; set; } = null!;

    public bool Gender { get; set; } = true;

    public DateTime Birthday { get; set; }
}