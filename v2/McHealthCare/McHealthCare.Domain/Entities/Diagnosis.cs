namespace McHealthCare.Domain.Entities
{
    public partial class Diagnosis : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public Guid? DiseaseCategoryId { get; set; }
        public Guid? CronisCategoryId { get; set; }

        
        public virtual DiseaseCategory? DiseaseCategory { get; set; }

        
        public virtual CronisCategory? CronisKategory { get; set; }
    }
}