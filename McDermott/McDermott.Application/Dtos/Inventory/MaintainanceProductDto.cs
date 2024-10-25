using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class MaintainanceProductDto : IMapFrom<MaintainanceProduct>
    {
        public long Id {  get; set; }
        public long? MaintainanceId { get; set; }
        public long? ProductId { get; set; }
        public string? SerialNumber { get; set; }
        public string? Note { get; set; }
        public DateTime? Expired { get; set; }
        public EnumStatusMaintainance? Status { get; set; }

        [SetToNull]
        public virtual MaintainanceDto? Maintainance { get; set; }
        [SetToNull]
        public virtual ProductDto? Product { get; set; }

    }
}
