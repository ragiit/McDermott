using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ReceivingStockDto : IMapFrom<ReceivingStock>
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Please Selected Destination!!..")]
        public long? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? Reference { get; set; }
        public string? StatusReceived { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }
    }
}