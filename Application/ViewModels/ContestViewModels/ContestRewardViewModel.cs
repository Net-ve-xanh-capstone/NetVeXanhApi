using Application.ViewModels.AccountViewModels;

namespace Application.ViewModels.ContestViewModels;

public class ContestRewardViewModel
{
    public string Name { get; set; }
    public List<AccountRewardViewModel> ListAccount { get; set; }
}