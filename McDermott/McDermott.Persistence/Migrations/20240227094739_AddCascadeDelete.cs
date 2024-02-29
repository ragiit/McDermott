using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
