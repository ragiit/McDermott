namespace McDermott.Domain.Entities
{
    public partial class Diagnosis : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public long? DiseaseCategoryId { get; set; }
        public long? CronisCategoryId { get; set; }

        public virtual DiseaseCategory? DiseaseCategory { get; set; }
        public virtual CronisCategory? CronisKategory { get; set; }
    }
}