using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PaintingViewModels
{
    public class PaintingForScheduleViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string TopicName { get; set; }
        public string CompetitorCode { get; set; }
        public string Reason {  get; set; }
        public string Code { get; set; }
        public string AwardId { get; set; }
        public string? Award { get; set; }
        public bool IsJudged {  get; set; }

    }
}
