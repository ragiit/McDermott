using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class Accident : BaseAuditableEntity
    {
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
        public EnumStatusAccident Status { get; set; } = EnumStatusAccident.Draft;
        public string? EmployeeDescription { get; set; }
        public string AccidentLocation { get; set; } = "Inside";

       

        #region Employee Cause Of Injury
        public List<string> SelectedEmployeeCauseOfInjury1 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury2 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury3 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury4 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury5 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury6 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury7 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury8 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury9 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury10 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury11 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury12 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury13 { get; set; } = [];
        public List<string> SelectedEmployeeCauseOfInjury14 { get; set; } = [];

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

        #endregion

        #region Nature Of Injury

        public List<string> SelectedNatureOfInjury1 { get; set; } = [];
        public List<string> SelectedNatureOfInjury2 { get; set; } = [];
        public List<string> SelectedNatureOfInjury3 { get; set; } = [];
        public List<string> SelectedNatureOfInjury4 { get; set; } = [];
        public List<string> SelectedNatureOfInjury5 { get; set; } = [];
        public List<string> SelectedNatureOfInjury6 { get; set; } = [];
        public List<string> SelectedNatureOfInjury7 { get; set; } = [];
        public List<string> SelectedNatureOfInjury8 { get; set; } = [];
        public string? NatureOfInjury1 { get; set; }
        public string? NatureOfInjury2 { get; set; }
        public string? NatureOfInjury3 { get; set; }
        public string? NatureOfInjury4 { get; set; }
        public string? NatureOfInjury5 { get; set; }
        public string? NatureOfInjury6 { get; set; }
        public string? NatureOfInjury7 { get; set; }
        public string? NatureOfInjury8 { get; set; }

        #endregion

        #region Part Of Body

        public List<string> SelectedPartOfBody1 { get; set; } = [];
        public List<string> SelectedPartOfBody2 { get; set; } = [];
        public List<string> SelectedPartOfBody3 { get; set; } = [];
        public List<string> SelectedPartOfBody4 { get; set; } = [];
        public List<string> SelectedPartOfBody5 { get; set; } = [];
        public List<string> SelectedPartOfBody6 { get; set; } = [];
        public List<string> SelectedPartOfBody7 { get; set; } = [];
        public List<string> SelectedPartOfBody8 { get; set; } = [];
        public List<string> SelectedPartOfBody9 { get; set; } = [];
        public List<string> SelectedPartOfBody10 { get; set; } = [];
        public List<string> SelectedPartOfBody11 { get; set; } = [];
        public List<string> SelectedPartOfBody12 { get; set; } = [];


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

        #endregion

        #region Treatment
        public List<string> SelectedTreatment1 { get; set; } = [];
        public List<string> SelectedTreatment2 { get; set; } = [];
        public List<string> SelectedTreatment3 { get; set; } = [];
        public List<string> SelectedTreatment4 { get; set; } = [];
        public List<string> SelectedTreatment5 { get; set; } = [];
        public List<string> SelectedTreatment6 { get; set; } = [];
        public List<string> SelectedTreatment7 { get; set; } = [];
        public string? Treatment1 { get; set; }
        public string? Treatment2 { get; set; }
        public string? Treatment3 { get; set; }
        public string? Treatment4 { get; set; }
        public string? Treatment5 { get; set; }
        public string? Treatment6 { get; set; }
        public string? Treatment7 { get; set; }
        #endregion

        [SetToNull]
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }

        [SetToNull]
        public virtual User? Employee { get; set; }
        [SetToNull]
        public virtual Department? Department { get; set; }

        [SetToNull]
        public virtual User? SafetyPersonnel { get; set; }
    }
}
