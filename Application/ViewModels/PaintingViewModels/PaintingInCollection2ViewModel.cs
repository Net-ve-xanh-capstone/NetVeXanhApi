using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PaintingViewModels
{
    public class PaintingInCollection2ViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TopicId { get; set; }
        public string TopicName { get; set; }
        public string ContestName { get; set; }
        public string Code { get; set; }
        public string OwnerName { get; set; }
    }
}
