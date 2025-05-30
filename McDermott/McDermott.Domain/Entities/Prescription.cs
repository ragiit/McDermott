﻿namespace McDermott.Domain.Entities
{
    public class Prescription : BaseAuditableEntity
    {
        public long PharmacyId { get; set; }
        public long? DrugFromId { get; set; }
        public long? DrugRouteId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? ProductId { get; set; }
        public long? UomId { get; set; }
        public long? SignaId { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public string? DosageFrequency { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        public long? GivenAmount { get; set; } // jumlah yg diberikan
        public long? PriceUnit { get; set; }

        [SetToNull]
        public DrugForm? DrugForm { get; set; }

        [SetToNull]
        public Pharmacy? Pharmacy { get; set; }

        [SetToNull]
        public Product? Product { get; set; }

        [SetToNull]
        public Signa? Signa { get; set; }

        [SetToNull]
        public DrugRoute? DrugRoute { get; set; }

        [SetToNull]
        public DrugDosage? DrugDosage { get; set; }

        [SetToNull]
        public MedicamentGroup? MedicamentGroup { get; set; }

        [SetToNull]
        public List<ActiveComponent>? ActiveComponent { get; set; }

        [SetToNull]
        public List<StockOutPrescription>? StockOutPrescription { get; set; }
    }
}