using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService.IValidationService
{
    public interface IAccountValidationService
    {
        Task<bool> IsExistedId(Guid id);
        Task<bool> IsExistedCompetitor(Guid id);

        Task<bool> IsExistStaff(Guid id);
        Task<bool> IsExistPhone(string phone);
        Task<bool> IsExistEmail(string email);
        Task<bool> IsExistUsername(string username);
    }
}
