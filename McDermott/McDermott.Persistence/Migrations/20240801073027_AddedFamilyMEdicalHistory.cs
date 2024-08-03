using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFamilyMEdicalHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FamilyMedicalHistory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyMedicalHistoryOther",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsFamilyMedicalHistory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsMedicationHistory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationHistory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastMedicalHistory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyMedicalHistory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FamilyMedicalHistoryOther",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsFamilyMedicalHistory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMedicationHistory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MedicationHistory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PastMedicalHistory",
                table: "Users");
        }
    }
}
