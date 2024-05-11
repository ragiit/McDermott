namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultantClinicalAssesmentDto : IMapFrom<GeneralConsultantClinicalAssesment>
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public double _Weight { get; set; }

        public double Weight
        {
            get => _Weight;
            set
            {
                _Weight = value;
                CalculateBMI(Height, value);
            }
        }

        public double _Height { get; set; }

        public double Height
        {
            get => _Height;
            set
            {
                _Height = value;
                CalculateBMI(value, Weight);
            }
        }

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

        public long RR { get; set; }
        public long Temp { get; set; }

        [Required]
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

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        public virtual AwarenessDto? AwarenessDto { get; set; }
        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}