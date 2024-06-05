﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Base;

namespace Domain.Models
{
    public class EducationalLevel : BaseModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string EducationLevel { get; set; }
        public int ContestId {  get; set; }

        //Relation
        public ICollection<Award> Award { get; set; }
        public Contest Contest { get; set; }
        public ICollection<Round> Round { get; set; }
    }
}
