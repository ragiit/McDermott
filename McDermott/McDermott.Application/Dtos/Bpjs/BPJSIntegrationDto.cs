using McDermott.Domain.Common;
using System.Text.Json.Serialization;

namespace McDermott.Application.Dtos.Bpjs
{
    public class BPJSIntegrationDto : IMapFrom<BPJSIntegration>
    {
        public long Id { get; set; }
        public long? InsurancePolicyId { get; set; }
        [JsonPropertyName("noKartu")]
        public string? NoKartu { get; set; }

        [JsonPropertyName("nama")]
        public string? Nama { get; set; }

        [JsonPropertyName("hubunganKeluarga")]
        public string? HubunganKeluarga { get; set; }

        [JsonPropertyName("sex")]
        public string? Sex { get; set; }

        [JsonPropertyName("tglLahir")]
        public DateTime? TglLahir { get; set; }

        [JsonPropertyName("tglMulaiAktif")]
        public DateTime? TglMulaiAktif { get; set; }

        [JsonPropertyName("tglAkhirBerlaku")]
        public DateTime? TglAkhirBerlaku { get; set; }

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

        [JsonPropertyName("tunggakan")]
        public int Tunggakan { get; set; }
        public string? KdProviderPstKdProvider { get; set; }
        public string? KdProviderPstNmProvider { get; set; }

        public string? KdProviderGigiKdProvider { get; set; }
        public string? KdProviderGigiNmProvider { get; set; }
        public string? JnsKelasNama { get; set; }
        public string? JnsKelasKode { get; set; }
        public string? JnsPesertaNama { get; set; }
        public string? JnsPesertaKode { get; set; }
        public string? AsuransiKdAsuransi { get; set; }
        public string? AsuransiNmAsuransi { get; set; }
        public string? AsuransiNoAsuransi { get; set; }
        public bool AsuransiCob { get; set; }

        [SetToNull]
        public InsurancePolicyDto? InsurancePolicy { get; set; }
    }
}
