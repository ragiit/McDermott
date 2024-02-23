using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultantClinicalAssesmentDto : IMapFrom<GeneralConsultantClinicalAssesment>
    {
        public int Id { get; set; }
        public int GeneralConsultanServiceId { get; set; }
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
        public int RR { get; set; }
        public int Temp { get; set; }
        [Required]
        public int HR { get; set; }
        public int RBS { get; set; }
        public int Systolic{ get; set; }
        public int DiastolicBP { get; set; }
        public int SpO2 { get; set; }

        public double BMIIndex { get; set; }
        public string BMIIndexString { get; set; } = "0";

        public string BMIState { get; set; } = "-";

        public int E { get; set; }
        public int V { get; set; }
        public int M { get; set; }

        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}
