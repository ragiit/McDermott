using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class BenefitConfiguration :BaseAuditableEntity
    {
        
        public string? BenefitName { get; set; }

        public EnumBenefitType? TypeOfBenefit { get; set; } // Enum for 'Qty' or 'Amount'

    
        public int? BenefitValue { get; set; }

        public EnumBenefitDurationType? DurationOfBenefit { get; set; } // Enum for 'Days', 'Months', 'Years'

        public int? BenefitDuration { get; set; }

        public EnumEligibilityType? Eligibility { get; set; } // Enum for 'Employee' or 'Non-Employee'

        public EnumBenefitStatus? Status { get; set; } // Enum for 'Draft', 'Active', 'Non-Active'

    }
}
