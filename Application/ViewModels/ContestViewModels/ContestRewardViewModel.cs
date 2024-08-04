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
        public string Name { get; set; }
        public List<AccountRewardViewModel> AccountReward { get; set; }
    }
}
