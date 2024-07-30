using Microsoft.AspNetCore.Identity;

namespace McHealthCare.Domain.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public Guid? GroupId { get; set; }  
        public string? MartialStatus { get; set; }
        public string? PlaceOfBirth { get; set; } 
        public DateTime? DateOfBirth { get; set; }

        public virtual Patient? Patient { get; set; }  // Navigation property
        public virtual Employee? Employee { get; set; }  // Navigation property
        public virtual Doctor? Doctor { get; set; }  // Navigation property

    }
}