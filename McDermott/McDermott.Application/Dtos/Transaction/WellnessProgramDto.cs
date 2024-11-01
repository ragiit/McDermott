using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramDto : IMapFrom<WellnessProgram>
    {
        public long Id { get; set; }
        public long DiagnosisId { get; set; }
        public string? ProgramName { get; set; }
        public string? Category { get; set; }
        public List<string> SelectedDiagnoses { get; set; } = [];
        public int SeverityLevel { get; set; }
        public string? ProgramContent { get; set; }
        public string? Status { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool HasSpecialSessions { get; set; }

        public DiagnosisDto? Diagnosis { get; set; }
    }

    public class CreateUpdateWellnessProgramDto  
    {
        public long Id { get; set; }
        public long DiagnosisId { get; set; }
        public string? ProgramName { get; set; }
        public string? Category { get; set; }
        public List<string> SelectedDiagnoses { get; set; } = [];
        public int SeverityLevel { get; set; }
        public string? ProgramContent { get; set; }
        public string? Status { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool HasSpecialSessions { get; set; }
         
    }
}
