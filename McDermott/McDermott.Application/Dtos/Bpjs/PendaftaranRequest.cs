namespace McDermott.Application.Dtos.Bpjs
{
    public class PendaftaranRequest
    {
        public string kdProviderPeserta { get; set; }
        public string tglDaftar { get; set; }
        public string noKartu { get; set; }
        public string kdPoli { get; set; }
        public object? keluhan { get; set; } = null;
        public bool kunjSakit { get; set; } = true;
        public int sistole { get; set; } = 0;
        public int diastole { get; set; } = 0;
        public int beratBadan { get; set; } = 0;
        public int tinggiBadan { get; set; } = 0;
        public int respRate { get; set; } = 0;
        public int lingkarPerut { get; set; } = 0;
        public int heartRate { get; set; } = 0;
        public int rujukBalik { get; set; } = 0;
        public string kdTkp { get; set; } = "10";
    }
}