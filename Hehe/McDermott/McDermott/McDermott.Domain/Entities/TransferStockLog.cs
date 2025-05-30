﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransferStockLog : BaseAuditableEntity
    {
        public long? TransferStockId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? UserById { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public TransferStock? TransferStock { get; set; }

        [SetToNull]
        public Locations? Source { get; set; }

        [SetToNull]
        public Locations? Destination { get; set; }

        [SetToNull]
        public User? UserBy { get; set; }
    }
}