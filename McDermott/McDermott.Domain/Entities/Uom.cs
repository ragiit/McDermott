using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    public class Uom : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public long? UomCategoryId { get; set; }

        [NotMapped]
        private string? _type;
        public string? Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (value is not null && value == "Reference Unit of Measure for this category")
                {
                    BiggerRatio = null;
                }
            }
        }

        //public string? Multiple { get; set; }

        public float? BiggerRatio { get; set; }

        public bool Active { get; set; } = true;
        public float? RoundingPrecision { get; set; }

        [SetToNull]
        public virtual UomCategory? UomCategory { get; set; }

        [SetToNull]
        public virtual List<ActiveComponent>? ActiveComponents { get; set; }

        public List<Medicament>? Medicaments { get; set; }
    }
}
