using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignatureAndConfined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abdomen",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AbdomenCircumference",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Bp",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cardiovascular",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChestCircumference",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEximinedbyDoctor",
                table: "GeneralConsultanMedicalSupports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMedialHistory",
                table: "GeneralConsultanMedicalSupports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EarNoseThroat",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnteringConfinedSpaceCount",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ExaminedPhysicianId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extremities",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Eye",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Height",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAsthmaOrLungAilment",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackPainOrLimitationOfMobility",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaustrophobia",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefectiveSenseOfSmell",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDiabetesOrHypoglycemia",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEyesightProblem",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFaintingSpellOrSeizureOrEpilepsy",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstTimeEnteringConfinedSpace",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHearingDisorder",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeartDiseaseOrDisorder",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHighBloodPressure",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLowerLimbsDeformity",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMeniereDiseaseOrVertigo",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Musculoskeletal",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neurologic",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Pulse",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recommendeds",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "RemarksMedicalHistory",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Respiratory",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespiratoryFitTest",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RespiratoryRate",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SignatureEmployeeId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SignatureEmployeeImagesMedicalHistory",
                table: "GeneralConsultanMedicalSupports",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SignatureEximinedDoctor",
                table: "GeneralConsultanMedicalSupports",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpirometryTest",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Temperature",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Wt",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abdomen",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "AbdomenCircumference",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Bp",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Cardiovascular",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "ChestCircumference",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "DateEximinedbyDoctor",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "DateMedialHistory",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "EarNoseThroat",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "EnteringConfinedSpaceCount",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "ExaminedPhysicianId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Extremities",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Eye",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsAsthmaOrLungAilment",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsBackPainOrLimitationOfMobility",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsClaustrophobia",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsDefectiveSenseOfSmell",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsDiabetesOrHypoglycemia",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsEyesightProblem",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsFaintingSpellOrSeizureOrEpilepsy",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsFirstTimeEnteringConfinedSpace",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsHearingDisorder",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsHeartDiseaseOrDisorder",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsHighBloodPressure",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsLowerLimbsDeformity",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsMeniereDiseaseOrVertigo",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Musculoskeletal",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Neurologic",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Pulse",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Recommendeds",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "RemarksMedicalHistory",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Respiratory",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "RespiratoryFitTest",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "RespiratoryRate",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "SignatureEmployeeId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "SignatureEmployeeImagesMedicalHistory",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "SignatureEximinedDoctor",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "SpirometryTest",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Wt",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
