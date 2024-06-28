using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigrateClinicalAssesmentToGeneralConsultant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppoimentDate",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "NoRM",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "BirthDay",
                table: "GeneralConsultanServices",
                newName: "AppointmentDate");

            migrationBuilder.AddColumn<long>(
                name: "AwarenessId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BMIIndex",
                table: "GeneralConsultanServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BMIIndexString",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BMIState",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClinicVisitTypes",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Diastole",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DiastolicBP",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "E",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "HR",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "GeneralConsultanServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "M",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PainScale",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RR",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Sistole",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SpO2",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Systolic",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Temp",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "V",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WaistCircumference",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "GeneralConsultanServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_AwarenessId",
                table: "GeneralConsultanServices",
                column: "AwarenessId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Awarenesses_AwarenessId",
                table: "GeneralConsultanServices",
                column: "AwarenessId",
                principalTable: "Awarenesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Awarenesses_AwarenessId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_AwarenessId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "AwarenessId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "BMIIndex",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "BMIIndexString",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "BMIState",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ClinicVisitTypes",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Diastole",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "DiastolicBP",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "E",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "HR",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "M",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "PainScale",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "RR",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Sistole",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "SpO2",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Systolic",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "V",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "WaistCircumference",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "GeneralConsultanServices",
                newName: "BirthDay");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppoimentDate",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoRM",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
