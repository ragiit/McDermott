using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveBase64AccidentToGc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "ImageToBase64",
                table: "Accidents");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageToBase64",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ImageToBase64",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageToBase64",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
