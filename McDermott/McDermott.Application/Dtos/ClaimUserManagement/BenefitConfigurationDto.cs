namespace McDermott.Application.Dtos.ClaimUserManagement
{
    public class BenefitConfigurationDto : IMapFrom<BenefitConfiguration>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Benefit Name is required")]
        public string? BenefitName { get; set; }

        [Required(ErrorMessage = "Benefit Value is required.")]
        public string? TypeOfBenefit { get; set; } // Enum for 'Qty' or 'Amount'

        public int? BenefitValue { get; set; }

        public string? DurationOfBenefit { get; set; } // Enum for 'Days', 'Months', 'Years'

        public int? BenefitDuration { get; set; }

        [Required(ErrorMessage = "Benefit Eligibility is required.")]
        public bool? IsEmployee { get; set; } = false;// Enum for 'Employee' or 'Non-Employee'

        public EnumBenefitStatus? Status { get; set; } // Enum for 'Draft', 'Active', 'Non-Active'
    }
}