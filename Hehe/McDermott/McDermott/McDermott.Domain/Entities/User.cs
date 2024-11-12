using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    public partial class User : BaseAuditableEntity
    { 
        [Required]
        public string Email {  get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty; 

    }
}