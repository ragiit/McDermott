﻿namespace McDermott.Application.Dtos.Medical
{
    public partial class CronisCategoryDto : IMapFrom<CronisCategory>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
    }
}