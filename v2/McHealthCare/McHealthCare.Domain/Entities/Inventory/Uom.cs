using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities.Inventory
{
    public class Uom : BaseAuditableEntity
    {
        public Guid? UomCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

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

        public float? BiggerRatio { get; set; }

        public bool Active { get; set; } = true;
        public float? RoundingPrecision { get; set; }

        public virtual UomCategory? UomCategory { get; set; }
    }
}