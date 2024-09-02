using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSystemParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AntreanFKTPBaseURL",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AntreanFKTPBaseURLValue",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsId",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdAplikasi",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCareBaseURL",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCareBaseURLValue",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCareCodeProvider",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserKey",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "SystemParameters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntreanFKTPBaseURL",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "AntreanFKTPBaseURLValue",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "ConsId",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "KdAplikasi",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "PCareBaseURL",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "PCareBaseURLValue",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "PCareCodeProvider",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "SystemParameters");
        }
    }
}
