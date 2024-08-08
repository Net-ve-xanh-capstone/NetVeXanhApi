using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoundJudgingCriteria
    {      
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid RoundId {  get; set; }
        public Guid JudgingCriteriaId {  get; set; }


        public Round Round { get; set; }
        public JudgingCriteria JudgingCriteria { get; set; }
    }
}
