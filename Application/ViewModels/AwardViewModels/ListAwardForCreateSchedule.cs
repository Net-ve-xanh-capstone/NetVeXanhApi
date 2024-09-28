using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AwardViewModels
{
    public class ListAwardForCreateSchedule
    {
        public int paintingForSchedule { get; set; }
        public List<AwardViewResponse> listAward { get; set; }
    }
}
