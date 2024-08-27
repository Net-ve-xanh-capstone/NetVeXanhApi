using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AwardRankPriority
    {
        private readonly List<string> _priority = new List<string> { "Giải Nhất", "Giải Nhì", "Giải Ba", "Giải Khuyến Khích" };

        public int GetRankPriority(string rank)
        {
            var index = _priority.IndexOf(rank);
            return index >= 0 ? index : int.MaxValue;
        }
    }
}
