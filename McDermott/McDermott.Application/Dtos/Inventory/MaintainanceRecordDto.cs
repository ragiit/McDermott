using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public partial class MaintenanceRecordDto : IMapFrom<MaintenanceRecord>
    {
        public long Id { get; set; }
        public long? MaintenanceId { get; set; }
        public long? ProductId { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentName { get; set; }
        public string? SequenceProduct { get; set; }

        [SetToNull]
        public virtual MaintenanceDto? Maintenance { get; set; }
        [SetToNull]
        public virtual ProductDto? Product { get; set; }
    }
}
