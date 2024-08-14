using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Schedule
{
    public class RatingFinalRoundForWeb
    {
        public Guid ScheduleId { get; set; }
        public Guid? AwardId { get; set; }
        public Guid PaintingId { get; set; }
        public string Reason { get; set; }
    }
}
