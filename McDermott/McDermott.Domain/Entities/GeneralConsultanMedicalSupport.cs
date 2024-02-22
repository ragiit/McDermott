using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultanMedicalSupport : BaseAuditableEntity
    {
        public int? GeneralConsultanServiceId { get; set; }

        public string? LabEximinationName { get; set; }
        public string? LabEximinationAttachment { get; set; }
        public string? RadiologyEximinationName { get; set; }
        public string? RadiologyEximinationAttachment { get; set; }

        public string? AlcoholEximinationName { get; set; }
        public string? AlcoholEximinationAttachment { get; set; }
        public bool? AlcoholNegative { get; set; }
        public bool? AlcoholPositive { get; set; }

        public string? DrugEximinationName { get; set; }
        public string? DrugEximinationAttachment { get; set; }
        public bool? DrugNegative { get; set; }
        public bool? DrugPositive { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}
