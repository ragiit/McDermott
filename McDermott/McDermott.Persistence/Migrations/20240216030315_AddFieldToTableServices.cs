using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToTableServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsKiosk",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPatient",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsKiosk",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsPatient",
                table: "Services");
        }
    }
}
