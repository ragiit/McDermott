using McHealthCare.Module.BusinessObjects.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Module.BusinessObjects.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual Guid? UpdatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
    }
}