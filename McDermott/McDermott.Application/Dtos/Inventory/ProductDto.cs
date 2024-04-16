using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ProductDto : IMapFrom<Product>
    {
        public long id { get; set; }
        public string? Name { get; set; }
    }
}
