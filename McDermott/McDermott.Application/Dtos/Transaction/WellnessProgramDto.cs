﻿using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramDto : IMapFrom<WellnessProgram>
    {
        public long Id { get; set; }
        public long? AwarenessEduCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public EnumWellness Status { get; set; } = EnumWellness.Draft;
        public string StatusString => Status.GetDisplayName();

        [SetToNull]
        public virtual AwarenessEduCategoryDto? AwarenessEduCategory { get; set; }
    }

    public class CreateUpdateWellnessProgramDto
    {
        public long Id { get; set; }
        public long? AwarenessEduCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public EnumWellness Status { get; set; } = EnumWellness.Draft;
    }
}