namespace McDermott.Application.Dtos.Inventory
{
    public class InventoryAdjusmentDto : IMapFrom<InventoryAdjusment>
    {
        public long Id { get; set; }

        [Required]
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public EnumStatusInventoryAdjusment Status { get; set; } = EnumStatusInventoryAdjusment.Draft;
        public string StatusName => Status.GetDisplayName();


        public LocationDto? Location { get; set; }
        public CompanyDto? Company { get; set; }
    }
}
