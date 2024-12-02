using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GCReferToInternalDto:IMapFrom<GCReferToInternal>
    {
        public long Id { get; set; }
        public long? GeneralConsultanServiceId { get; set; }
        public string? TypeClaim { get; set; }
        public string? Number { get; set; }
        public DateTime DateRJMCINT { get; set; } = DateTime.Now;
        public string? ReferTo { get; set; }
        public string? Hospital { get; set; }
        public string? Specialist { get; set; }
        public string? CategoryRJMCINT { get; set; }
        public string? ExamFor { get; set; }
        public long? OccupationalId { get; set; }
        public string? TempDiagnosis { get; set; }
        public string? TherapyProvide { get; set; }
        public string? InpatientClass { get; set; }

        [SetToNull]
        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}
