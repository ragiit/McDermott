using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class delServicessConfigKiosk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KioskConfigs_Services_ServiceId",
                table: "KioskConfigs");

            migrationBuilder.DropIndex(
                name: "IX_KioskConfigs_ServiceId",
                table: "KioskConfigs");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "KioskConfigs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "KioskConfigs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KioskConfigs_ServiceId",
                table: "KioskConfigs",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_KioskConfigs_Services_ServiceId",
                table: "KioskConfigs",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
