using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class GeneralInformationDto: IMapFrom<GeneralInformation>
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? BpjsClasificationId { get; set; }
        public long? UomId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public string? PurchaseUom { get; set; }
        public string? ProductType { get; set; }
        public string? InputType { get; set; }
        public string? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public string? Cost { get; set; }
        public string? InternalReference { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }
        [SetToNull]
        public virtual UomDto? Uom { get; set; }
        [SetToNull]
        public virtual ProductCategoryDto? ProductCategory { get; set; }
        [SetToNull]
        public virtual CompanyDto? Company { get; set; }
    }
}
