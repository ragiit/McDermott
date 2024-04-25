using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeletePatientAllergy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Users_UserId",
                table: "PatientAllergies");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Users_UserId",
                table: "PatientAllergies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Users_UserId",
                table: "PatientAllergies");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Users_UserId",
                table: "PatientAllergies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
