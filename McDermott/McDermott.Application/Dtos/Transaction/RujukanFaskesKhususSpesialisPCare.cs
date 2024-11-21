using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Transaction
{
    public class RujukanFaskesKhususSpesialisPCare
    {
        [JsonProperty("kdppk")]
        public string Kdppk { get; set; }

        [JsonProperty("nmppk")]
        public string Nmppk { get; set; }

        [JsonProperty("alamatPpk")]
        public string AlamatPpk { get; set; }

        [JsonProperty("telpPpk")]
        public string TelpPpk { get; set; }

        [JsonProperty("kelas")]
        public string Kelas { get; set; }

        [JsonProperty("nmkc")]
        public string Nmkc { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("jadwal")]
        public string Jadwal { get; set; }

        [JsonProperty("jmlRujuk")]
        public int JmlRujuk { get; set; }

        [JsonProperty("kapasitas")]
        public int Kapasitas { get; set; }

        [JsonProperty("persentase")]
        public int Persentase { get; set; }
    }
}