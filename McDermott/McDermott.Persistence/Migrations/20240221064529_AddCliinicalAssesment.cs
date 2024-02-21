using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCliinicalAssesment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateSchendule",
                table: "GeneralConsultanServices",
                newName: "RegistrationDate");

            migrationBuilder.AddColumn<int>(
                name: "InsurancePolicyId",
                table: "GeneralConsultanServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleTime",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeMedical",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeneralConsultantClinicalAssesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultantServiceId = table.Column<int>(type: "int", nullable: true),
                    _Weight = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    _Height = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    StagingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RR = table.Column<int>(type: "int", nullable: false),
                    Temp = table.Column<int>(type: "int", nullable: false),
                    HR = table.Column<int>(type: "int", nullable: false),
                    RBS = table.Column<int>(type: "int", nullable: false),
                    SystolicDiastolicBP = table.Column<int>(type: "int", nullable: false),
                    SpO2 = table.Column<int>(type: "int", nullable: false),
                    BMIIndex = table.Column<double>(type: "float", nullable: false),
                    BMIIndexString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BMIState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    E = table.Column<int>(type: "int", nullable: false),
                    V = table.Column<int>(type: "int", nullable: false),
                    M = table.Column<int>(type: "int", nullable: false), 
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultantClinicalAssesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                        column: x => x.GeneralConsultantServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_InsurancePolicyId",
                table: "GeneralConsultanServices",
                column: "InsurancePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultantServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_InsurancePolicies_InsurancePolicyId",
                table: "GeneralConsultanServices",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_InsurancePolicies_InsurancePolicyId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropTable(
                name: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_InsurancePolicyId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "InsurancePolicyId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "TypeMedical",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "GeneralConsultanServices",
                newName: "DateSchendule");
        }
    }
}
