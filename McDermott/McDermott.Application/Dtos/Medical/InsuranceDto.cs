namespace McDermott.Application.Dtos.Medical
{
    public class InsuranceDto : IMapFrom<Insurance>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Type { get; set; }

        private bool _isBPJSKesehatan = true;

        public bool IsBPJSKesehatan
        {
            get => _isBPJSKesehatan;
            set
            {
                if (_isBPJSKesehatan != value)
                {
                    _isBPJSKesehatan = value;
                    IsBPJSTK = !_isBPJSKesehatan;
                }
            }
        }

        public bool IsMcu { get; set; } = false;

        private bool _isBPJSTK;

        public bool IsBPJSTK
        {
            get => _isBPJSTK;
            set
            {
                if (_isBPJSTK != value)
                {
                    _isBPJSTK = value;
                    _isBPJSKesehatan = !_isBPJSTK;
                }
            }
        }

        public long? AdminFee { get; set; }
        public long? Presentase { get; set; }
        public long? AdminFeeMax { get; set; }
    }
}