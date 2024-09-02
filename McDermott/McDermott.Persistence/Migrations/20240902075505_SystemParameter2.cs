using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SystemParameter2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntreanFKTPBaseURLValue",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "PCareBaseURLValue",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "SystemParameters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AntreanFKTPBaseURLValue",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PCareBaseURLValue",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
