using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllergyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PatientAllergyIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodPatientAllergyIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "PharmacologyPatientAllergyIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "WeatherPatientAllergyIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodPatientAllergyIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PharmacologyPatientAllergyIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WeatherPatientAllergyIds",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PatientAllergyIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
