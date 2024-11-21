using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class MaintenanceProductDto : IMapFrom<MaintenanceProduct>
    {
        public long Id { get; set; }
        public long? MaintenanceId { get; set; }
        public long? ProductId { get; set; }
        public string? SerialNumber { get; set; }
        public string? Note { get; set; }
        public string? Title { get; set; }
        public string? RequestBy { get; set; }
        public string? ResponsBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? Expired { get; set; }
        public EnumStatusMaintenance? Status { get; set; }

        [SetToNull]
        public virtual MaintenanceDto? Maintenance { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }
    }
}