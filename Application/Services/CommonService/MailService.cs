using System.Net;
using System.Net.Mail;
using Application.BaseModels;
using Application.IService.ICommonService;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Services.CommonService;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;
    private readonly string _templateDirectory;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _templateDirectory = Path.Combine(AppContext.BaseDirectory, configuration["EmailTemplateDirectory"]);
    }


    public async Task SendEmail(MailModel request)
    {
        var emailHost = _configuration["Email:EmailHost"];
        var emailUsername = _configuration["Email:EmailUsername"];
        var emailPassword = _configuration["Email:EmailPassword"];

        var fromAddress = new MailAddress(emailUsername);
        var toAddress = new MailAddress(request.To);

        var smtpClient = new SmtpClient(emailHost, 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(emailUsername, emailPassword),
            EnableSsl = true
        };

        var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(message);
    }

    public async Task SendAccountInformation(Account account, string password)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");

        template = template.Replace("[Tên Thí Sinh]", account.FullName);
        template = template.Replace("[Mật khẩu]", password);
        template = template.Replace("[Tên tài khoản]", account.Username);

        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "THÔNG TIN ĐĂNG NHẬP";
        mail.Body = body;
        await SendEmail(mail);
    }

    public async Task PassPreliminaryRound(Account account)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");

        template = template.Replace("[Tên Thí Sinh]", account.FullName);

        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "THÔNG BÁO CUỘC THI NÉT VẼ XANH";
        mail.Body = body;
        await SendEmail(mail);
    }

    public string GetEmailTemplate(string templateName)
    {
        var templatePath = Path.Combine(_templateDirectory, templateName);
        return File.ReadAllText(templatePath);
    }

    public async Task BanAccount(Account account)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");
        template = template.Replace("[Tên người dùng]", account.FullName);

        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "Thông Báo Khóa Tài Khoản";
        mail.Body = body;
        await SendEmail(mail);
    }

    public async Task NotificationForFinalRound(Account account, Round round)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");

        template = template.Replace("[Tên Thí Sinh]", account.FullName);

        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "THÔNG BÁO VÒNG CHUNG KẾT NÉT VẼ XANH";
        mail.Body = body;
        await SendEmail(mail);
    }

    public async Task ResetPassword(Account account)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");

        template = template.Replace("[Tên Thí Sinh]", account.FullName);
        template = template.Replace("[Mật khẩu]", account.Password);
        template = template.Replace("[Tên tài khoản]", account.Username);


        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "CẬP NHẬT MẬT KHẨU";
        mail.Body = body;
        await SendEmail(mail);
    }

    public async Task UnBanAccount(Account account)
    {
        var template = GetEmailTemplate("SendAccountForCompetitor.html");

        template = template.Replace("[Tên Thí Sinh]", account.FullName);
        template = template.Replace("[Mật khẩu]", account.Password);
        template = template.Replace("[Tên tài khoản]", account.Username);


        var supportmail = _configuration["NetVeXanh:SupportMail"];
        var supportphone = _configuration["NetVeXanh:SupportPhone"];
        template = template.Replace("[email hỗ trợ]", supportmail);
        template = template.Replace("[số điện thoại hỗ trợ]", supportphone);

        var body = template;

        var mail = new MailModel();
        mail.To = account.Email;
        mail.Subject = "THÔNG BÁO MỞ KHÓA TÀI KHOẢN";
        mail.Body = body;
        await SendEmail(mail);
    }
}