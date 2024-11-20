namespace McDermott.Domain.Entities
{
    public partial class Diagnosis : BaseAuditableEntity
    {
        public string Name { get; set; }

        public string? NameInd { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public long? DiseaseCategoryId { get; set; }
        public long? CronisCategoryId { get; set; }

        [SetToNull]
        public virtual DiseaseCategory? DiseaseCategory { get; set; }

        [SetToNull]
        public virtual CronisCategory? CronisCategory { get; set; }
    }
}