using Application.BaseModels;
using Domain.Models;

namespace Application.IService.ICommonService;

public interface IMailService
{
    Task SendEmail(MailModel request);
    Task SendAccountInformation(Account account, string password);
    Task PassPreliminaryRound(Account account);
}