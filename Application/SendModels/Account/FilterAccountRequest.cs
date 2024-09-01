using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.Account
{
    public class FilterAccountRequest
    {
        public string? ContestId { get; set; }
        public string? LevelId { get; set; }
        public string? RoundName { get; set; }
        public bool IsHavePrize {  get; set; }

    }
}
