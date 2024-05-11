using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultantClinicalAssesment : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }

        [NotMapped]
        public double _Weight { get; set; }

        public double Weight
        {
            get; set;
        }

        [NotMapped]
        public double _Height { get; set; }

        public double Height
        {
            get; set;
        }

        public long RR { get; set; }
        public long Temp { get; set; }
        public long HR { get; set; }
        public long PainScale { get; set; }
        public long Systolic { get; set; }
        public long DiastolicBP { get; set; }
        public long SpO2 { get; set; }

        public double BMIIndex { get; set; }
        public string BMIIndexString { get; set; } = "0";

        public string BMIState { get; set; } = "-";
        public string? ClinicVisitTypes { get; set; }
        public long? AwarenessId { get; set; }

        public long E { get; set; }
        public long V { get; set; }
        public long M { get; set; }

        public void CalculateBMI(double height, double weight)
        {
            BMIIndex = 0;
            BMIState = "-";

            if (height > 0 && weight > 0)
            {
                weight = weight / 100;
                height = height / 1000;

                BMIIndex = weight / (height * height);
                BMIIndexString = Math.Round(BMIIndex, 2).ToString();

                if (BMIIndex < 18.5)
                {
                    BMIState = "Low Weight";
                }
                else if (BMIIndex >= 18.5 && BMIIndex <= 24.9)
                {
                    BMIState = "Normal";
                }
                else if (BMIIndex == 25.0 && BMIIndex <= 29.9)
                {
                    BMIState = "Overweight";
                }
                else
                {
                    BMIState = "Obesity";
                }
                return;
            }
        }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }

        [SetToNull]
        public virtual Awareness? Awareness { get; set; }
    }
}