using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedReferenceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_LocationId",
                table: "GeneralConsultanServices",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Locations_LocationId",
                table: "GeneralConsultanServices",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Locations_LocationId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_LocationId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "GeneralConsultanServices");
        }
    }
}
