using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Schedule
{
    public class CreateScheduleAutoAssignRequest
    {
        public string? Description { get; set; }
        public Guid RoundId { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> ListExaminer { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
