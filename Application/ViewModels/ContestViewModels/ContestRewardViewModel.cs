using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.AccountViewModels;

namespace Application.ViewModels.ContestViewModels
{
    public class ContestRewardViewModel
    {
        public string Name { get; set;}
        public List<AwardContestRewardViewModel> AwardContestReward { get; set; }
    }
    public class AwardContestRewardViewModel
    {
        public string Level { get; set; }
        public string? Rank { get; set; }
        public List<AccountRewardViewModel> AccountReward {  get; set; } 
    }
}
