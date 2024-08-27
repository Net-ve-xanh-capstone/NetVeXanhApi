using Application.BaseModels;
using Domain.Models;

namespace Application.IService.ICommonService;

public interface IMailService
{
    Task SendEmail(MailModel request);
    Task SendAccountInformation(Account account, string password);
    Task SendConfirmRegistration(Account account);
    Task SendConfirmSubmitPainting(Account account, Painting painting);
    Task PassPreliminaryRound(Painting painting, Round round);
    Task RejectPainting(Painting painting);
    Task PassFinalRound(Painting painting, Round round);
    Task NotPassPreliminaryRound(Painting painting, Round round);
    Task SendScheduleToExaminer(Account account);
}