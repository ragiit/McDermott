using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ReceivingLogDto :IMapFrom<ReceivingLog>
    {
        public long Id { get; set; }
        public long? ReceivingId { get; set; }
        public long? SourceId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ReceivingStockDto? Receiving { get; set; }
        public UserDto? UserBy { get; set; }
        public LocationDto? Source { get; set; }
    }
}
