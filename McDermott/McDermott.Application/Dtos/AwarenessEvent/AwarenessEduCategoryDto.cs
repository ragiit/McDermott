using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class AwarenessEduCategoryDto : IMapFrom<AwarenessEduCategory>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}