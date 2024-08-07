namespace Application.SendModels.AccountSendModels;

public class AccountUpdateRequest
{
    public Guid Id { get; set; }
    public DateTime Birthday { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string? Avatar { get; set; }
    public bool Gender { get; set; }
}