using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Extentions.EnumHelper;

namespace McDermott.Domain.Entities
{
    public class EducationProgram : BaseAuditableEntity
    {
       
        public string? EventName { get; set; } 
               
        public long? EventCategoryId { get; set; } 
        public string? HTMLContent { get; set; }

        public string? Description { get; set; }

        public string? HTMLMaterial {  get; set; }

        public string? EventLink { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? MaterialContent { get; set; }
        public string? Attendance { get; set; }
        public string? Slug { get; set; }
        public EnumStatusEducationProgram? Status { get; set; }

        public AwarenessEduCategory? EventCategory { get; set; }
    }
}
