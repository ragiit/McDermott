using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Bpjs
{
    public class KunjunganRequest
    {
        [JsonProperty("noKunjungan")]
        public object NoKunjungan { get; set; }

        [JsonProperty("noKartu")]
        public string NoKartu { get; set; }

        [JsonProperty("tglDaftar")]
        public string TglDaftar { get; set; }

        [JsonProperty("kdPoli")]
        public object KdPoli { get; set; }

        [JsonProperty("keluhan")]
        public string Keluhan { get; set; }

        [JsonProperty("kdSadar")]
        public string KdSadar { get; set; }

        [JsonProperty("sistole")]
        public int Sistole { get; set; }

        [JsonProperty("diastole")]
        public int Diastole { get; set; }

        [JsonProperty("beratBadan")]
        public int BeratBadan { get; set; }

        [JsonProperty("tinggiBadan")]
        public int TinggiBadan { get; set; }

        [JsonProperty("respRate")]
        public int RespRate { get; set; }

        [JsonProperty("heartRate")]
        public int HeartRate { get; set; }

        [JsonProperty("lingkarPerut")]
        public int LingkarPerut { get; set; }

        [JsonProperty("kdStatusPulang")]
        public string KdStatusPulang { get; set; }

        [JsonProperty("tglPulang")]
        public string TglPulang { get; set; }

        [JsonProperty("kdDokter")]
        public string KdDokter { get; set; }

        [JsonProperty("kdDiag1")]
        public string KdDiag1 { get; set; }

        [JsonProperty("kdDiag2")]
        public object KdDiag2 { get; set; }

        [JsonProperty("kdDiag3")]
        public object KdDiag3 { get; set; }

        [JsonProperty("kdPoliRujukInternal")]
        public object? KdPoliRujukInternal { get; set; } = null;

        [JsonProperty("rujukLanjut")]
        public RujukLanjutRequest? RujukLanjut { get; set; } = null;

        [JsonProperty("kdTacc")]
        public int KdTacc { get; set; }

        [JsonProperty("alasanTacc")]
        public object? AlasanTacc { get; set; } = null;

        [JsonProperty("anamnesa")]
        public string? Anamnesa { get; set; } = null;

        [JsonProperty("alergiMakan")]
        public string AlergiMakan { get; set; } = "00";

        [JsonProperty("alergiUdara")]
        public string AlergiUdara { get; set; } = "00";

        [JsonProperty("alergiObat")]
        public string AlergiObat { get; set; } = "00";

        [JsonProperty("kdPrognosa")]
        public string KdPrognosa { get; set; } = "01";

        [JsonProperty("terapiObat")]
        public string TerapiObat { get; set; } = null;

        [JsonProperty("terapiNonObat")]
        public string TerapiNonObat { get; set; } = null;

        [JsonProperty("bmhp")]
        public string Bmhp { get; set; } = null;

        [JsonProperty("suhu")]
        public string Suhu { get; set; }
    }
}