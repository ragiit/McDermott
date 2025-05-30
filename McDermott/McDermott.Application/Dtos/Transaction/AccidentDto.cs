﻿using FluentValidation;

namespace McDermott.Application.Dtos.Transaction
{
    public class AccidentDto : IMapFrom<Accident>
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public long SafetyPersonnelId { get; set; }

        [Display(Name = "Date Of Occurrence")]
        public DateTime DateOfOccurrence { get; set; } = DateTime.Now;

        public DateTime DateOfFirstTreatment { get; set; } = DateTime.Now;

        public bool RibbonSpecialCase { get; set; } = false;
        public string? Sent { get; set; }
        public string? EmployeeClass { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? EstimatedDisability { get; set; }

        public string? AreaOfYard { get; set; }
        public long? ProjectId { get; set; }

        public EnumStatusAccident Status { get; set; } = EnumStatusAccident.Draft;
        public string StatusName => Status.GetDisplayName();
        public string? EmployeeDescription { get; set; }
        public string AccidentLocation { get; set; } = "Inside";

        #region Employee Cause Of Injury

        public IEnumerable<string> SelectedEmployeeCauseOfInjury1 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury2 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury3 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury4 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury5 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury6 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury7 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury8 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury9 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury10 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury11 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury12 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury13 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury14 { get; set; } = [];

        public string? EmployeeCauseOfInjury1 { get; set; }
        public string? EmployeeCauseOfInjury2 { get; set; }
        public string? EmployeeCauseOfInjury3 { get; set; }
        public string? EmployeeCauseOfInjury4 { get; set; }
        public string? EmployeeCauseOfInjury5 { get; set; }
        public string? EmployeeCauseOfInjury6 { get; set; }
        public string? EmployeeCauseOfInjury7 { get; set; }
        public string? EmployeeCauseOfInjury8 { get; set; }
        public string? EmployeeCauseOfInjury9 { get; set; }
        public string? EmployeeCauseOfInjury10 { get; set; }
        public string? EmployeeCauseOfInjury11 { get; set; }
        public string? EmployeeCauseOfInjury12 { get; set; }
        public string? EmployeeCauseOfInjury13 { get; set; }
        public string? EmployeeCauseOfInjury14 { get; set; }

        #endregion Employee Cause Of Injury

        #region Nature Of Injury

        public IEnumerable<string> SelectedNatureOfInjury1 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury2 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury3 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury4 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury5 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury6 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury7 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury8 { get; set; } = [];
        public string? NatureOfInjury1 { get; set; }
        public string? NatureOfInjury2 { get; set; }
        public string? NatureOfInjury3 { get; set; }
        public string? NatureOfInjury4 { get; set; }
        public string? NatureOfInjury5 { get; set; }
        public string? NatureOfInjury6 { get; set; }
        public string? NatureOfInjury7 { get; set; }
        public string? NatureOfInjury8 { get; set; }

        #endregion Nature Of Injury

        #region Part Of Body

        public IEnumerable<string> SelectedPartOfBody1 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody2 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody3 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody4 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody5 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody6 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody7 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody8 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody9 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody10 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody11 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody12 { get; set; } = [];

        public string? PartOfBody1 { get; set; }
        public string? PartOfBody2 { get; set; }
        public string? PartOfBody3 { get; set; }
        public string? PartOfBody4 { get; set; }
        public string? PartOfBody5 { get; set; }
        public string? PartOfBody6 { get; set; }
        public string? PartOfBody7 { get; set; }
        public string? PartOfBody8 { get; set; }
        public string? PartOfBody9 { get; set; }
        public string? PartOfBody10 { get; set; }
        public string? PartOfBody11 { get; set; }
        public string? PartOfBody12 { get; set; }

        #endregion Part Of Body

        #region Treatment

        public IEnumerable<string> SelectedTreatment1 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment2 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment3 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment4 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment5 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment6 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment7 { get; set; } = [];
        public string? Treatment1 { get; set; }
        public string? Treatment2 { get; set; }
        public string? Treatment3 { get; set; }
        public string? Treatment4 { get; set; }
        public string? Treatment5 { get; set; }
        public string? Treatment6 { get; set; }
        public string? Treatment7 { get; set; }

        #endregion Treatment

        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public virtual UserDto? Employee { get; set; }
        public virtual DepartmentDto? Department { get; set; }
        public virtual UserDto? SafetyPersonnel { get; set; }
        public virtual Project? Project { get; set; }
    }

    public class CreateUpdateAccidentDto
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public long SafetyPersonnelId { get; set; }

        [Display(Name = "Date Of Occurrence")]
        [Required]
        public DateTime? DateOfOccurrence { get; set; }

        [Required]
        public DateTime? DateOfFirstTreatment { get; set; }

        public bool RibbonSpecialCase { get; set; } = false;
        public string? Sent { get; set; }
        public string? EmployeeClass { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? EstimatedDisability { get; set; }

        public string? AreaOfYard { get; set; }
        public long? ProjectId { get; set; }

        public EnumStatusAccident Status { get; set; } = EnumStatusAccident.Draft;
        public string StatusName => Status.GetDisplayName();
        public string? EmployeeDescription { get; set; }
        public string AccidentLocation { get; set; } = "Inside";

        #region Employee Cause Of Injury

        public IEnumerable<string> SelectedEmployeeCauseOfInjury1 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury2 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury3 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury4 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury5 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury6 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury7 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury8 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury9 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury10 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury11 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury12 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury13 { get; set; } = [];
        public IEnumerable<string> SelectedEmployeeCauseOfInjury14 { get; set; } = [];

        public string? EmployeeCauseOfInjury1 { get; set; }
        public string? EmployeeCauseOfInjury2 { get; set; }
        public string? EmployeeCauseOfInjury3 { get; set; }
        public string? EmployeeCauseOfInjury4 { get; set; }
        public string? EmployeeCauseOfInjury5 { get; set; }
        public string? EmployeeCauseOfInjury6 { get; set; }
        public string? EmployeeCauseOfInjury7 { get; set; }
        public string? EmployeeCauseOfInjury8 { get; set; }
        public string? EmployeeCauseOfInjury9 { get; set; }
        public string? EmployeeCauseOfInjury10 { get; set; }
        public string? EmployeeCauseOfInjury11 { get; set; }
        public string? EmployeeCauseOfInjury12 { get; set; }
        public string? EmployeeCauseOfInjury13 { get; set; }
        public string? EmployeeCauseOfInjury14 { get; set; }

        #endregion Employee Cause Of Injury

        #region Nature Of Injury

        public IEnumerable<string> SelectedNatureOfInjury1 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury2 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury3 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury4 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury5 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury6 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury7 { get; set; } = [];
        public IEnumerable<string> SelectedNatureOfInjury8 { get; set; } = [];
        public string? NatureOfInjury1 { get; set; }
        public string? NatureOfInjury2 { get; set; }
        public string? NatureOfInjury3 { get; set; }
        public string? NatureOfInjury4 { get; set; }
        public string? NatureOfInjury5 { get; set; }
        public string? NatureOfInjury6 { get; set; }
        public string? NatureOfInjury7 { get; set; }
        public string? NatureOfInjury8 { get; set; }

        #endregion Nature Of Injury

        #region Part Of Body

        public IEnumerable<string> SelectedPartOfBody1 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody2 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody3 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody4 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody5 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody6 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody7 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody8 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody9 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody10 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody11 { get; set; } = [];
        public IEnumerable<string> SelectedPartOfBody12 { get; set; } = [];

        public string? PartOfBody1 { get; set; }
        public string? PartOfBody2 { get; set; }
        public string? PartOfBody3 { get; set; }
        public string? PartOfBody4 { get; set; }
        public string? PartOfBody5 { get; set; }
        public string? PartOfBody6 { get; set; }
        public string? PartOfBody7 { get; set; }
        public string? PartOfBody8 { get; set; }
        public string? PartOfBody9 { get; set; }
        public string? PartOfBody10 { get; set; }
        public string? PartOfBody11 { get; set; }
        public string? PartOfBody12 { get; set; }

        #endregion Part Of Body

        #region Treatment

        public IEnumerable<string> SelectedTreatment1 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment2 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment3 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment4 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment5 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment6 { get; set; } = [];
        public IEnumerable<string> SelectedTreatment7 { get; set; } = [];
        public string? Treatment1 { get; set; }
        public string? Treatment2 { get; set; }
        public string? Treatment3 { get; set; }
        public string? Treatment4 { get; set; }
        public string? Treatment5 { get; set; }
        public string? Treatment6 { get; set; }
        public string? Treatment7 { get; set; }

        #endregion Treatment
    }

    public class AccidentGeneralValidator : AbstractValidator<GeneralConsultanServiceDto>
    {
        public AccidentGeneralValidator()
        {
            // Validation for Patient field
            RuleFor(x => x.Patient)
                .NotEmpty().WithMessage("Patient field is required");

            RuleFor(x => x.Patient.CurrentMobile)
                .NotEmpty().WithMessage("Current Mobile field is required")
                .When(x => x.Patient != null).WithMessage("Patient is required before checking Current Mobile");

            RuleFor(x => x.TypeRegistration)
                .NotEmpty().WithMessage("Registration Type field is required");

            RuleFor(x => x.Payment)
                .NotEmpty().WithMessage("Payment field is required");

            RuleFor(x => x.RegistrationDate)
                .NotEmpty().WithMessage("Registration Date field is required");

            RuleFor(x => x.Payment)
                .NotEmpty().WithMessage("Payment field is required");

            When(x => x.Payment == "Insurance" || x.Payment == "BPJS", () =>
            {
                RuleFor(x => x.InsurancePolicyId)
                    .NotNull().WithMessage("Insurance Policy is required when Payment Method is Insurance or BPJS.");
            });
        }
    }

    public class AccidentValidator : AbstractValidator<AccidentDto>
    {
        public AccidentValidator()
        {
            // Validation for Patient field
            RuleFor(x => x.SafetyPersonnelId)
                .NotEmpty().WithMessage("Safety Personel field is required");

            RuleFor(x => x.ProjectId)
              .NotEmpty().WithMessage("Project field is required");
        }
    }
}