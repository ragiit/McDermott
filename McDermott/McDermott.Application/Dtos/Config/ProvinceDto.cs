﻿namespace McDermott.Application.Dtos.Config
{
    public class ProvinceDto : IMapFrom<Province>
    {
        public long Id { get; set; }

        [Required]
        public long? CountryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string Code { get; set; } = string.Empty; // State Code

        public CountryDto? Country { get; set; }
    }

    public class CreateUpdateProvinceDto
    {
        public long Id { get; set; }
        public long? CountryId { get; set; }
        public string Name { get; set; } = string.Empty; // State Name
        public string Code { get; set; } = string.Empty; // State Code
    }
}