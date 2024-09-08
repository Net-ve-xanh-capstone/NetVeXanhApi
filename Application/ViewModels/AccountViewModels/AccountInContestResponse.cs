using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountViewModels
{
    public class AccountInContestResponse
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public DateTime Birthday { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string Code { get; set; }
        public string? Phone { get; set; }
        public bool Gender { get; set; }

        //Thêm

    }
}
