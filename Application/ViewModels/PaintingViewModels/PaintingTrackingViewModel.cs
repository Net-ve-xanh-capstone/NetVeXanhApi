using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PaintingViewModels
{
    public class PaintingTrackingViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? SubmittedTime { get; set; }
        public DateTime? ReviewedTime { get; set; }
        public DateTime? FinalDecisionTime { get; set; }
    }
}
