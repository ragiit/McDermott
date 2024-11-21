namespace McDermott.Domain.Entities
{
    public class BenefitConfiguration : BaseAuditableEntity
    {
        public string? BenefitName { get; set; }

        public string? TypeOfBenefit { get; set; } // Enum for 'Qty' or 'Amount'

        public int? BenefitValue { get; set; }

        public string? DurationOfBenefit { get; set; } // Enum for 'Days', 'Months', 'Years'

        public int? BenefitDuration { get; set; }

        public bool? IsEmployee { get; set; } // Enum for 'Employee' or 'Non-Employee'

        public EnumBenefitStatus? Status { get; set; } // Enum for 'Draft', 'Active', 'Non-Active'
    }
}