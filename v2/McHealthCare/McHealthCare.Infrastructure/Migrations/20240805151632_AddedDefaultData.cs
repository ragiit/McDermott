using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultData",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultData",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultData",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Villages_DistrictId",
                table: "Villages",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_Districts_DistrictId",
                table: "Villages",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Villages_Districts_DistrictId",
                table: "Villages");

            migrationBuilder.DropIndex(
                name: "IX_Villages_DistrictId",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "IsDefaultData",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IsDefaultData",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "IsDefaultData",
                table: "AspNetUsers");
        }
    }
}
