using System.Text.Json.Serialization;

namespace McDermott.Application.Dtos.Bpjs
{
    public class BPJSIntegrationDto
    {
        [JsonPropertyName("noKartu")]
        public string? NoKartu { get; set; }

        [JsonPropertyName("nama")]
        public string? Nama { get; set; }

        [JsonPropertyName("hubunganKeluarga")]
        public string? HubunganKeluarga { get; set; }

        [JsonPropertyName("sex")]
        public string? Sex { get; set; }

        [JsonPropertyName("tglLahir")]
        public string? TglLahir { get; set; }

        [JsonPropertyName("tglMulaiAktif")]
        public string? TglMulaiAktif { get; set; }

        [JsonPropertyName("tglAkhirBerlaku")]
        public string? TglAkhirBerlaku { get; set; }

        [JsonPropertyName("kdProviderPst")]
        public KdProviderPst KdProviderPstt { get; set; } = new();

        [JsonPropertyName("kdProviderGigi")]
        public KdProviderGigi KdProviderGigii { get; set; } = new();

        [JsonPropertyName("jnsKelas")]
        public JnsKelas JnsKelass { get; set; } = new();

        [JsonPropertyName("jnsPeserta")]
        public JnsPeserta JnsPesertaa { get; set; } = new();

        [JsonPropertyName("golDarah")]
        public string? GolDarah { get; set; }

        [JsonPropertyName("noHP")]
        public string? NoHP { get; set; }

        [JsonPropertyName("noKTP")]
        public string? NoKTP { get; set; }

        [JsonPropertyName("pstProl")]
        public string? PstProl { get; set; }

        [JsonPropertyName("pstPrb")]
        public string? PstPrb { get; set; }

        [JsonPropertyName("aktif")]
        public bool Aktif { get; set; }

        [JsonPropertyName("ketAktif")]
        public string? KetAktif { get; set; }

        [JsonPropertyName("asuransi")]
        public Asuransi Asuransii { get; set; } = new();

        [JsonPropertyName("tunggakan")]
        public int Tunggakan { get; set; }

        public class Asuransi
        {
            [JsonPropertyName("kdAsuransi")]
            public string? KdAsuransi { get; set; }

            [JsonPropertyName("nmAsuransi")]
            public string? NmAsuransi { get; set; }

            [JsonPropertyName("noAsuransi")]
            public string? NoAsuransi { get; set; }

            [JsonPropertyName("cob")]
            public bool Cob { get; set; }
        }

        public class JnsKelas
        {
            [JsonPropertyName("nama")]
            public string? Nama { get; set; }

            [JsonPropertyName("kode")]
            public string? Kode { get; set; }
        }

        public class JnsPeserta
        {
            [JsonPropertyName("nama")]
            public string? Nama { get; set; }

            [JsonPropertyName("kode")]
            public string? Kode { get; set; }
        }

        public class KdProviderGigi
        {
            [JsonPropertyName("kdProvider")]
            public string? KdProvider { get; set; }

            [JsonPropertyName("nmProvider")]
            public string? NmProvider { get; set; }
        }

        public class KdProviderPst
        {
            [JsonPropertyName("kdProvider")]
            public string? KdProvider { get; set; }

            [JsonPropertyName("nmProvider")]
            public string? NmProvider { get; set; }
        }
    }
}
