namespace McDermott.Application.Dtos.Medical
{
    public partial class ProcedureDto : IMapFrom<Procedure>
    {
         public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Code_Test { get; set; }

        [StringLength(200)]
        public string? Classification { get; set; }
    }
}