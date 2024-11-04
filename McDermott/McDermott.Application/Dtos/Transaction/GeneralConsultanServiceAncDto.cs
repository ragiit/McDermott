namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanServiceAncDto : IMapFrom<GeneralConsultanServiceAnc>
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public string Reference { get; set; } = string.Empty;
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

        public virtual UserDto? Patient { get; set; }
        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }    
    }

    public class CreateUpdateGeneralConsultanServiceAncDto
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public long GeneralConsultanServiceId { get; set; }
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
