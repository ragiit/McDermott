using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class capeEy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropIndex(
                name: "IX_DetailQueueDisplays_CounterId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropIndex(
                name: "IX_DetailQueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.RenameColumn(
                name: "QueueDisplayId",
                table: "DetailQueueDisplays",
                newName: "ServicekId");

            migrationBuilder.RenameColumn(
                name: "CounterId",
                table: "DetailQueueDisplays",
                newName: "ServiceId");

            migrationBuilder.AddColumn<long>(
                name: "KioskQueueId",
                table: "DetailQueueDisplays",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NumberQueue",
                table: "DetailQueueDisplays",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KioskQueueId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropColumn(
                name: "NumberQueue",
                table: "DetailQueueDisplays");

            migrationBuilder.RenameColumn(
                name: "ServicekId",
                table: "DetailQueueDisplays",
                newName: "QueueDisplayId");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "DetailQueueDisplays",
                newName: "CounterId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailQueueDisplays_CounterId",
                table: "DetailQueueDisplays",
                column: "CounterId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailQueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays",
                column: "QueueDisplayId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays",
                column: "CounterId",
                principalTable: "Counters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
