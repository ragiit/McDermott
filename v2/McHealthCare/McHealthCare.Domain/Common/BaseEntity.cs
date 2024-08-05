using McHealthCare.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }
    }
}