﻿namespace McDermott.Application.Dtos.Pharmacies
{
    public class UomCategoryDto : IMapFrom<UomCategory>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Type { get; set; }
    }
}