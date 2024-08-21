using Application.ViewModels.AccountViewModels;

namespace Application.ViewModels.ContestViewModels;

public class ContestRewardResponse
{
    public string Name { get; set; }
    public List<AccountRewardResponse> ListAccount { get; set; }
}