using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.IService.ICommonService
{
    public interface ISortAwardService
    {
        List<Award> SortAwards(List<Award> awards);
    }
}
