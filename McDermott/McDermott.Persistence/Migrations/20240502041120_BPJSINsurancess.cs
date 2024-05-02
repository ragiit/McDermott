using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BPJSINsurancess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPJSIntegrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePolicyId = table.Column<long>(type: "bigint", nullable: true),
                    NoKartu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HubunganKeluarga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TglLahir = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglMulaiAktif = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglAkhirBerlaku = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GolDarah = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoHP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstProl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstPrb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    KetAktif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tunggakan = table.Column<int>(type: "int", nullable: false),
                    KdProviderPstKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderPstNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiKdAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNmAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNoAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiCob = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPJSIntegrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                        column: x => x.InsurancePolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPJSIntegrations_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPJSIntegrations");
        }
    }
}
