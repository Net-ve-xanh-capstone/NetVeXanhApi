namespace Application.ViewModels.AccountViewModels;

public class AccountViewModel
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Avatar { get; set; }
    public DateTime Birthday { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string Code { get; set; }
    public string? Phone { get; set; }
    public bool Gender { get; set; } 
    public string Status {  get; set; }
}
