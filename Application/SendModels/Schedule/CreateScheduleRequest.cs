using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Schedule
{
    public class CreateScheduleRequest
    {
        public string? Description { get; set; }
        public Guid RoundId { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ExaminerId { get; set; }
        public int JudgedCount { get; set; }
        public Guid CurrentUserId { get; set; }
        public List<PrizeWithCountViewModel> Awards { get; set; }
    }
}
