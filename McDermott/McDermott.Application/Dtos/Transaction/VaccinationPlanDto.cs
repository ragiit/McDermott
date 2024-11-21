namespace McDermott.Application.Dtos.Transaction
{
    public class VaccinationPlanDto : IMapFrom<VaccinationPlan>
    {
        public long Id { get; set; }
        public long? GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public long ProductId { get; set; }
        public long? PhysicianId { get; set; }
        public long? SalesPersonId { get; set; }
        public long? EducatorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public long TeoriticalQty { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string? Batch { get; set; }
        public int Dose { get; set; }
        public DateTime? NextDoseDate { get; set; }
        public string? Observations { get; set; }
        public EnumStatusVaccination Status { get; set; } = EnumStatusVaccination.Scheduled;

        [NotMapped]
        public string StatusName => Status.GetDisplayName();

        public UserDto? Patient { get; set; }
        public UserDto? Physician { get; set; }
        public UserDto? SalesPerson { get; set; }
        public UserDto? Educator { get; set; }
        public ProductDto? Product { get; set; }
        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }

    public class CreateUpdateVaccinationPlanDto
    {
        public long Id { get; set; }
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

        [NotMapped]
        public string StatusName => Status.GetDisplayName();
    }
}