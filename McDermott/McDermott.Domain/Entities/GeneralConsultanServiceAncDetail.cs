﻿namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanServiceAncDetail : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceAncId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Trimester { get; set; } = string.Empty;
        public string? Complaint { get; set; }
        public int KU { get; set; } // GeneralCondition
        public int TD { get; set; } // BloodPressure
        public int TD2 { get; set; } // BloodPressure
        public int Suhu { get; set; } // Temperature
        public int BB { get; set; } // Temperature
        public int UK { get; set; } // Temperature
        public int TFU { get; set; } // Temperature
        public string? FetusPosition { get; set; }  // Letak Janin
        public string? DJJ { get; set; } // Temperature
        public int TT { get; set; } // Temperature
        public string? InspectionInitials { get; set; }   // Paraf Pemeriksaan
        public bool IsReadOnly { get; set; } = false;

        public virtual GeneralConsultanServiceAnc? GeneralConsultanServiceAnc { get; set; }
    }
}