using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Ja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_EmployeeId",
                table: "GeneralConsultanMedicalSupports",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_EmployeeId",
                table: "GeneralConsultanMedicalSupports",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_EmployeeId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_EmployeeId",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
