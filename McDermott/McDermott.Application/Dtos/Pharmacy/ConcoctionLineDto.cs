namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionLineDto : IMapFrom<ConcoctionLine>
    {
        public long Id { get; set; }
        public long? ConcoctionId { get; set; }
        public long? MedicamentId { get; set; }
        //public long? UomDoseId { get; set; }
        public long? ActiveComponentId { get; set; }
        public long? MedicineDosage { get; set; }
        public long? QtyDose { get; set; }
        public long? MedicineUnitUomId { get; set; }
        public long? UnitUomId { get; set; }
        public long? TotalQty { get; set; }
        public long? AvailableQty { get; set; }

        public UomDto? UomDose
        { get; set; }

        public ActiveComponentDto? ActiveComponent { get; set; }
        public UomDto? MedicineUnitUom { get; set; }
        public MedicamentDto? Medicament { get; set; }
        public ConcoctionDto? Concoction { get; set; }
    }
}