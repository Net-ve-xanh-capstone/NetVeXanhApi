﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SendModels.EducationalLevel
{
    public class EducationalLevelUpdateRequest
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string EducationLevel { get; set; }
        public Guid ContestId { get; set; }
    }
}
