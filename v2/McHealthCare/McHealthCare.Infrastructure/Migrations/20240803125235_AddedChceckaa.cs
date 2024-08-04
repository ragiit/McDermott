using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedChceckaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCreate",
                table: "GroupMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "GroupMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsImport",
                table: "GroupMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "GroupMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdate",
                table: "GroupMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCreate",
                table: "GroupMenus");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "GroupMenus");

            migrationBuilder.DropColumn(
                name: "IsImport",
                table: "GroupMenus");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "GroupMenus");

            migrationBuilder.DropColumn(
                name: "IsUpdate",
                table: "GroupMenus");
        }
    }
}