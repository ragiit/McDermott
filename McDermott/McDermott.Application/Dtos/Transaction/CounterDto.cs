﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class CounterDto : IMapFrom<Counter>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? PhysicianId { get; set; }
        public string? Status { get; set; } = string.Empty;


    }
}
