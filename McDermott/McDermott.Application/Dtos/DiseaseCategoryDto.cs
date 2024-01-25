using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public partial class DiseaseCategoryDto:IMapFrom<DiseaseCategory>
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public ParentCategory? ParentCategory { get; set; }
    }
}
