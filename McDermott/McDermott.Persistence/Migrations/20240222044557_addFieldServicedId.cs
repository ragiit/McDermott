using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFieldServicedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServicedId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServicedId",
                table: "Services",
                column: "ServicedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services",
                column: "ServicedId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServicedId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServicedId",
                table: "Services");
        }
    }
}
