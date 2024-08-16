using System.Text.Json;
using System.Text.Json.Serialization;

namespace McHealthCare.Application.Dtos.Bpjs
{
    public class ResponseAPIBPJSIntegrationGetPeserta
    {
        public Guid Id { get; set; }

        [JsonPropertyName("noKartu")]
        public string? NoKartu { get; set; }

        [JsonPropertyName("nama")]
        public string? Nama { get; set; }

        [JsonPropertyName("hubunganKeluarga")]
        public string? HubunganKeluarga { get; set; }

        [JsonPropertyName("sex")]
        public string? Sex { get; set; }

        [JsonPropertyName("tglLahir")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? TglLahir { get; set; }

        [JsonPropertyName("tglMulaiAktif")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? TglMulaiAktif { get; set; }

        [JsonPropertyName("tglAkhirBerlaku")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? TglAkhirBerlaku { get; set; }

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

public class JsonDateConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string dateString = reader.GetString();

        if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            return date;
        }
        else
        {
            throw new JsonException($"The JSON value '{dateString}' is not in the expected format.");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("dd-MM-yyyy"));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}