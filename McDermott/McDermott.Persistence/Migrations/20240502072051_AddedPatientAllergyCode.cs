using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPatientAllergyCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmacologiCode",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FoodCode",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeatherCode",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmacologiCode",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "FoodCode",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "WeatherCode",
                table: "PatientAllergies");
        }
    }
}
