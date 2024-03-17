using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class KioskQueueId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays",
                column: "CounterId",
                principalTable: "Counters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
