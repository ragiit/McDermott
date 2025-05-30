﻿namespace McDermott.Application.Dtos.Pharmacies
{
    public class PharmacyLogDto : IMapFrom<PharmacyLog>
    {
        public long Id { get; set; }

        public long? PharmacyId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusPharmacy? status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public PharmacyDto? Pharmacy { get; set; }
        public UserDto? UserBy { get; set; }
    }
}