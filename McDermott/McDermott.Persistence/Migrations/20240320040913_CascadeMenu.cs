using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CascadeMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMenus_Menus_MenuId",
                table: "GroupMenus");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMenus_Menus_MenuId",
                table: "GroupMenus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMenus_Menus_MenuId",
                table: "GroupMenus");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMenus_Menus_MenuId",
                table: "GroupMenus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
