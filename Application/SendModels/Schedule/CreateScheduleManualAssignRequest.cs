using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Schedule
{
    public class CreateScheduleManualAssignRequest
    {
        public Guid RoundId { get; set; }
        public Guid CurrentUserId { get; set; }
        public List<ScheduleManualSingleExaminerRequest> listScheduleSingleExaminer {  get; set; }
    }
}
