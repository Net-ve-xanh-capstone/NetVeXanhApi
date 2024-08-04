using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountViewModels
{
    public class AccountRewardViewModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string Gender { get; set; }
        public string PaintingImage { get; set; }
        public string? Rank { get; set; }
    }
}
