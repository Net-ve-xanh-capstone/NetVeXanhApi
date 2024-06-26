﻿namespace Application.ViewModels.AccountViewModels;

public class AccountViewModel
{
    public string? UserName { get; set; }
    public DateTime Birthday { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool Gender { get; set; } = true;
}