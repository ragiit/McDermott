using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeleteGeneralConsultantServiceLabTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
