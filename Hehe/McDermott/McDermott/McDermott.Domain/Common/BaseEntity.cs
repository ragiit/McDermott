using McDermott.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}