namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanMedicalSupportDto : IMapFrom<GeneralConsultanMedicalSupport>
    {
        public int Id { get; set; }
        public int? GeneralConsultanServiceId { get; set; }

        public string? LabEximinationName { get; set; }
        public string? LabEximinationAttachment { get; set; }
        public string? RadiologyEximinationName { get; set; }
        public string? RadiologyEximinationAttachment { get; set; }

        public string? AlcoholEximinationName { get; set; }
        public string? AlcoholEximinationAttachment { get; set; }

        private bool _AlcoholNegative = false;
        private bool _AlcoholPositive = false;

        public bool AlcoholNegative
        {
            get => _AlcoholNegative;
            set
            {
                _AlcoholNegative = value;
                if (value)
                {
                    _AlcoholPositive = false;
                }
            }
        }

        public bool AlcoholPositive
        {
            get => _AlcoholPositive;
            set
            {
                _AlcoholPositive = value;
                if (value)
                {
                    _AlcoholNegative = false;
                }
            }
        }

        public string? DrugEximinationName { get; set; }
        public string? DrugEximinationAttachment { get; set; }

        private bool _DrugNegative = false;
        private bool _DrugPositive = false;

        public bool DrugNegative
        {
            get => _DrugNegative;
            set
            {
                _DrugNegative = value;
                if (value)
                {
                    _DrugPositive = false;
                }
            }
        }

        public bool DrugPositive
        {
            get => _DrugPositive;
            set
            {
                _DrugPositive = value;
                if (value)
                {
                    _DrugNegative = false;
                }
            }
        }

        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}