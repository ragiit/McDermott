using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class slepett : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.AlterColumn<long>(
                name: "QueueDisplayId",
                table: "DetailQueueDisplays",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CounterId",
                table: "DetailQueueDisplays",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "DetailQueueDisplays",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays",
                column: "CounterId",
                principalTable: "Counters",
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
                name: "FK_DetailQueueDisplays_Counters_CounterId",
                table: "DetailQueueDisplays");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailQueueDisplays_QueueDisplays_QueueDisplayId",
                table: "DetailQueueDisplays");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "QueueDisplays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "QueueDisplayId",
                table: "DetailQueueDisplays",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CounterId",
                table: "DetailQueueDisplays",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DetailQueueDisplays",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}