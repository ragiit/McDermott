using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Application.Dtos.Inventory
{
    public class UomDto : IMapFrom<Uom>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public long? UomCategoryId { get; set; }

        [NotMapped]
        private string _type = "Bigger than the reference Unit of Measure";
        public string Type
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

        public string? Multiple { get; set; }

        public float? BiggerRatio { get; set; }

        public bool Active { get; set; } = true;
        public float? RoundingPrecision { get; set; }

        public UomCategoryDto? UomCategory { get; set; }
    }
}
