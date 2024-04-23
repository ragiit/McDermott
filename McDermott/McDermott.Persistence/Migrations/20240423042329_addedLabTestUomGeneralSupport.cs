using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedLabTestUomGeneralSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LabTestId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabTestId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabTestId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "LabTestId",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
