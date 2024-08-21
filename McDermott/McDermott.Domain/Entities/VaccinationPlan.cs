using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class VaccinationPlan : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public long ProductId { get; set; }
        public long? PhysicianId { get; set; }
        public long? SalesPersonId { get; set; }
        public long? EducatorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string? Batch { get; set; }
        public int Dose { get; set; }
        public DateTime? NextDoseDate { get; set; }
        public string? Observations { get; set; }
        public EnumStatusVaccination Status { get; set; }

        public virtual User? Patient { get; set; }
        public virtual User? Physician { get; set; }
        public virtual User? SalesPerson { get; set; }
        public virtual User? Educator { get; set; }
        public virtual Product? Product { get; set; }
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}