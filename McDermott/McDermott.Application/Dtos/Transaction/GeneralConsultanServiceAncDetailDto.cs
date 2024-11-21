namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanServiceAncDetailDto : IMapFrom<GeneralConsultanServiceAncDetail>
    {
        public long Id { get; set; }
        public long? GeneralConsultanServiceAncId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
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
        public bool IsReadOnly { get; set; } = false;

        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }
    }

    public class CreateUpdateGeneralConsultanServiceAncDetailDto
    {
        public long Id { get; set; }
        public long? GeneralConsultanServiceAncId { get; set; }
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
        public bool IsReadOnly { get; set; } = false;
    }
}