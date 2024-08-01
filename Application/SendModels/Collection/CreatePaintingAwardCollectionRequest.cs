using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Collection
{
    public class CreatePaintingAwardCollectionRequest
    {
        public string Name { get; set; }
        public string? Image { get; set; } = null;
        public string Description { get; set; } = "Không có mô tả";
        public Guid CurrentUserId { get; set; }
        public Guid contestId { get; set; }
    }
}
