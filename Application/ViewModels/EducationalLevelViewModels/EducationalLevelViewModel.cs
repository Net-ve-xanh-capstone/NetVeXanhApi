﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.EducationalLevelViewModels
{
    public class EducationalLevelViewModel
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string EducationLevel { get; set; }
        public Guid? ContestId { get; set; }
    }
}
