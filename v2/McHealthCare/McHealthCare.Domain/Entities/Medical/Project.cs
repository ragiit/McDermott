﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Project : BaseAuditableEntity
    {
        public string? Name { get; set; } 
        public string? Code { get; set; }
    }
}
