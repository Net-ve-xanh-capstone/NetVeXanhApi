using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService.IValidationService
{
    public interface IRoundTopicValidationService
    {
        Task<bool> IsExistedId(Guid id);
    }
}
