using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBpjsIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPJSIntegrations");

            migrationBuilder.DropIndex(
                name: "IX_InsurancePolicies_NoCard",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "AgeAtTimeOfService",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "CardPrintDate",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "CurrentAge",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Diagnosa",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "DinSos",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Doctor",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "InsuranceNo",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "MedicalRecordNo",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoCard",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoId",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoSKTM",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NursingClass",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ParticipantName",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ParticipantStatus",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Poly",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Prolanis",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "PronalisPBR",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ServicePPKCode",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ServicePPKName",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ServiceParticipant",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "InsurancePolicies");

            migrationBuilder.RenameColumn(
                name: "TmtDate",
                table: "InsurancePolicies",
                newName: "TglMulaiAktif");

            migrationBuilder.RenameColumn(
                name: "TatDate",
                table: "InsurancePolicies",
                newName: "TglLahir");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "InsurancePolicies",
                newName: "TglAkhirBerlaku");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "InsurancePolicies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AsuransiCob",
                table: "InsurancePolicies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AsuransiKdAsuransi",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsuransiNmAsuransi",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsuransiNoAsuransi",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GolDarah",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HubunganKeluarga",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JnsKelasKode",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JnsKelasNama",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JnsPesertaKode",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JnsPesertaNama",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdProviderGigiKdProvider",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdProviderGigiNmProvider",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdProviderPstKdProvider",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdProviderPstNmProvider",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetAktif",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nama",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoHP",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoKTP",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoKartu",
                table: "InsurancePolicies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PstPrb",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PstProl",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tunggakan",
                table: "InsurancePolicies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_NoKartu",
                table: "InsurancePolicies",
                column: "NoKartu",
                unique: true,
                filter: "[NoKartu] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsurancePolicies_NoKartu",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "AsuransiCob",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "AsuransiKdAsuransi",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "AsuransiNmAsuransi",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "AsuransiNoAsuransi",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "GolDarah",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "HubunganKeluarga",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "JnsKelasKode",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "JnsKelasNama",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "JnsPesertaKode",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "JnsPesertaNama",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "KdProviderGigiKdProvider",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "KdProviderGigiNmProvider",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "KdProviderPstKdProvider",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "KdProviderPstNmProvider",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "KetAktif",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Nama",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoHP",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoKTP",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "NoKartu",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "PstPrb",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "PstProl",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "Tunggakan",
                table: "InsurancePolicies");

            migrationBuilder.RenameColumn(
                name: "TglMulaiAktif",
                table: "InsurancePolicies",
                newName: "TmtDate");

            migrationBuilder.RenameColumn(
                name: "TglLahir",
                table: "InsurancePolicies",
                newName: "TatDate");

            migrationBuilder.RenameColumn(
                name: "TglAkhirBerlaku",
                table: "InsurancePolicies",
                newName: "DateOfBirth");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "InsurancePolicies",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AgeAtTimeOfService",
                table: "InsurancePolicies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CardPrintDate",
                table: "InsurancePolicies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentAge",
                table: "InsurancePolicies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosa",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DinSos",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctor",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceName",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNo",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalRecordNo",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoCard",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoId",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoSKTM",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NursingClass",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantName",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantStatus",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "InsurancePolicies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poly",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prolanis",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PronalisPBR",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicePPKCode",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicePPKName",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceParticipant",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BPJSIntegrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePolicyId = table.Column<long>(type: "bigint", nullable: true),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    AsuransiCob = table.Column<bool>(type: "bit", nullable: false),
                    AsuransiKdAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNmAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNoAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GolDarah = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HubunganKeluarga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderPstKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderPstNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KetAktif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoHP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKartu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstPrb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstProl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TglAkhirBerlaku = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglLahir = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglMulaiAktif = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tunggakan = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_NoCard",
                table: "InsurancePolicies",
                column: "NoCard",
                unique: true,
                filter: "[NoCard] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BPJSIntegrations_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId");
        }
    }
}
