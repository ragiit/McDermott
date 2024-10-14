using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCPPTSubjektive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DiagnosisId",
                table: "GeneralConsultanCPPTs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NursingDiagnosesId",
                table: "GeneralConsultanCPPTs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Objective",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Planning",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subjective",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "GeneralConsultanCPPTs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanCPPTs_DiagnosisId",
                table: "GeneralConsultanCPPTs",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanCPPTs_NursingDiagnosesId",
                table: "GeneralConsultanCPPTs",
                column: "NursingDiagnosesId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanCPPTs_UserId",
                table: "GeneralConsultanCPPTs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanCPPTs_Diagnoses_DiagnosisId",
                table: "GeneralConsultanCPPTs",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanCPPTs_NursingDiagnoses_NursingDiagnosesId",
                table: "GeneralConsultanCPPTs",
                column: "NursingDiagnosesId",
                principalTable: "NursingDiagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanCPPTs_Users_UserId",
                table: "GeneralConsultanCPPTs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanCPPTs_Diagnoses_DiagnosisId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanCPPTs_NursingDiagnoses_NursingDiagnosesId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanCPPTs_Users_UserId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanCPPTs_DiagnosisId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanCPPTs_NursingDiagnosesId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanCPPTs_UserId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "NursingDiagnosesId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "Objective",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "Planning",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "Subjective",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GeneralConsultanCPPTs");
        }
    }
}
