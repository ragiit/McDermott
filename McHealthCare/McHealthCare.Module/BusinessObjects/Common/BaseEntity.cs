using McHealthCare.Module.BusinessObjects.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Module.BusinessObjects.Common
{
    public abstract class BaseEntity : IEntity
    {
        [Key, Column(Order = 0)]
        public virtual Guid Id { get; set; }
    }
}