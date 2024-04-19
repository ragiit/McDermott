using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedECGFieldMedicalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multiple",
                table: "Uoms");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndMaternityLeave",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaternityLeave",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartMaternityLeave",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AmphetaminesNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AmphetaminesPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BenzodiazepinesNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BenzodiazepinesPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CocaineMetabolitesNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CocaineMetabolitesPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ECGAttachment",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOtherExaminationECG",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MethamphetaminesNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MethamphetaminesPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpiatesNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpiatesPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherExaminationRemarkECG",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherExaminationTypeECG",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "THCCannabinoidMarijuanaNegative",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "THCCannabinoidMarijuanaPositive",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndMaternityLeave",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsMaternityLeave",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "StartMaternityLeave",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "AmphetaminesNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "AmphetaminesPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "BenzodiazepinesNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "BenzodiazepinesPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "CocaineMetabolitesNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "CocaineMetabolitesPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "ECGAttachment",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsOtherExaminationECG",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "MethamphetaminesNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "MethamphetaminesPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "OpiatesNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "OpiatesPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "OtherExaminationRemarkECG",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "OtherExaminationTypeECG",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "THCCannabinoidMarijuanaNegative",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "THCCannabinoidMarijuanaPositive",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.AddColumn<string>(
                name: "Multiple",
                table: "Uoms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
