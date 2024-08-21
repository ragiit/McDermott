using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class Maintainance : BaseAuditableEntity
    {
        public long? RequestById { get; set; }
        public string? Title { get; set; }
        public string? Sequence { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public long? ResponsibleById { get; set; }
        public long? EquipmentId {  get; set; }
        public string? SerialNumber { get; set; }
        public bool? isCorrective {  get; set; }
        public bool? isPreventive {  get; set; }
        public bool? isInternal { get; set; }
        public bool? isExternal { get; set; }
        public string? VendorBy { get; set; }
        public bool? Recurrent {  get; set; }
        public int? RepeatNumber {  get; set; }
        public EnumWorksDays? RepeatWork {  get; set; }
        public EnumStatusMaintainance? Status {  get; set; }
        public string? Note {  get; set; }

        [SetToNull]
        public virtual User? RequestBy { get; set; }
        [SetToNull]
        public virtual User? ResponsibleBy { get; set; }
        [SetToNull]
        public virtual Product? Equipment { get; set; }


    }
}
