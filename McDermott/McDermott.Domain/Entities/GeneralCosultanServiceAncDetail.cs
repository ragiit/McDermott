namespace McDermott.Domain.Entities
{
    public class GeneralCosultanServiceAncDetail : BaseAuditableEntity
    {
        public long GeneralCosultanServiceAncId { get; set; }
        public DateTime Date { get; set; }
        public string Trimester { get; set; } = string.Empty;
        public string? Complaint { get; set; }
        public int KU { get; set; } // GeneralCondition
        public int TD { get; set; } // BloodPressure
        public int Suhu { get; set; } // Temperature
        public int BB { get; set; } // Temperature
        public int UK { get; set; } // Temperature
        public int TFU { get; set; } // Temperature
        public string? FetusPosition { get; set; }  // Letak Janin
        public int DJJ { get; set; } // Temperature
        public int TT { get; set; } // Temperature
        public string? InspectionInitials { get; set; }   // Paraf Pemeriksaan

        public GeneralCosultanServiceAnc? GeneralCosultanServiceAnc { get; set; }
    }
}