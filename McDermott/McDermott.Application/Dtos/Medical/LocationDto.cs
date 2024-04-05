
namespace McDermott.Application.Dtos.Medical
{
    public class LocationDto : IMapFrom<Location>
    {
        public long Id { get; set; }

        public long? ParentLocationId { get; set; }
        public long? CompanyId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public string NameAndParent
        {
            get
            {
                if (ParentLocation is null)
                    return Name;

                return $"{Name}/{ParentLocation.Name}";
            }
        }

        [Required]
        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        public LocationDto? ParentLocation { get; set; }
        public CompanyDto? Company { get; set; }
    }
}