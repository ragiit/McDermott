using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldKioskQueues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceKId",
                table: "KioskQueues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_ServiceKId",
                table: "KioskQueues",
                column: "ServiceKId");

            migrationBuilder.AddForeignKey(
                name: "FK_KioskQueues_Services_ServiceKId",
                table: "KioskQueues",
                column: "ServiceKId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KioskQueues_Services_ServiceKId",
                table: "KioskQueues");

            migrationBuilder.DropIndex(
                name: "IX_KioskQueues_ServiceKId",
                table: "KioskQueues");

            migrationBuilder.DropColumn(
                name: "ServiceKId",
                table: "KioskQueues");
        }
    }
}
