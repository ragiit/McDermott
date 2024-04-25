namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionLineDto
    {
        public long Id { get; set; }
        public long? ConcoctionId { get; set; }
        public long? MedicamentId { get; set; }
        public long? UomDoseId { get; set; }
        public long? ActiveComponentId { get; set; }
        public float MedicineUnitOfDosage { get; set; }
        public float QtyDose { get; set; }
        public long? MedicineUnitUomId { get; set; }
        public long? UnitUomId { get; set; }
        public float TotalQty { get; set; }
        public float AvailableQty { get; set; }

        public UomDto? UomDose { get; set; }
        public ActiveComponentDto? ActiveComponent { get; set; }
        public UomDto? MedicineUnitUom { get; set; }
        public MedicamentDto? Medicament { get; set; }
        public ConcoctionDto? Concoction { get; set; }
    }
}
