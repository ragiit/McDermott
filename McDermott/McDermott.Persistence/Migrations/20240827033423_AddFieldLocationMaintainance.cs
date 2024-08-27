using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldLocationMaintainance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Maintainances",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_LocationId",
                table: "Maintainances",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintainances_Locations_LocationId",
                table: "Maintainances",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintainances_Locations_LocationId",
                table: "Maintainances");

            migrationBuilder.DropIndex(
                name: "IX_Maintainances_LocationId",
                table: "Maintainances");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Maintainances");
        }
    }
}
