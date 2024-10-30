using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class InventoryAdjustmentLogDto : IMapFrom<InventoryAdjusmentLog>
    {
        public long Id { get; set; }
        public long? InventoryAdjusmentId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusInventoryAdjustment Status { get; set; }

        [SetToNull]
        public InventoryAdjusmentDto? InventoryAdjusment { get; set; }
        [SetToNull]
        public User? UserBy { get; set; }

    }
}
