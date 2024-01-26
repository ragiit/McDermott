using System.ComponentModel.DataAnnotations;
namespace McDermott.Application.Dtos
{
    public partial class DiagnosisDto:IMapFrom<Diagnosis>
    {
        public int Id { get; set; }
        public int? DiseaseCategoryId { get; set; }
        public int? CronisCategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public DiseaseCategory? DiseaseCategory { get; set; }
        public CronisCategory? CronisCategory { get; set; }
    }
}
