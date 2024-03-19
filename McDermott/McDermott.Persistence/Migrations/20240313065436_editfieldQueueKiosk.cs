using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editfieldQueueKiosk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "KioskQueues",
                newName: "QueueStatus");

            migrationBuilder.RenameColumn(
                name: "NoQueue",
                table: "KioskQueues",
                newName: "QueueNumber");

            migrationBuilder.AddColumn<string>(
                name: "QueueStage",
                table: "KioskQueues",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueueStage",
                table: "KioskQueues");

            migrationBuilder.RenameColumn(
                name: "QueueStatus",
                table: "KioskQueues",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "QueueNumber",
                table: "KioskQueues",
                newName: "NoQueue");
        }
    }
}