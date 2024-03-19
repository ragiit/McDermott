using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedKioskQueueRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KioskQueueId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_KioskQueueId",
                table: "GeneralConsultanServices",
                column: "KioskQueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_KioskQueues_KioskQueueId",
                table: "GeneralConsultanServices",
                column: "KioskQueueId",
                principalTable: "KioskQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_KioskQueues_KioskQueueId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_KioskQueueId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "KioskQueueId",
                table: "GeneralConsultanServices");
        }
    }
}