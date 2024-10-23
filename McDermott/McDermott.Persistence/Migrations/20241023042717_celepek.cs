using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class celepek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserById",
                table: "TransferStockLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockLogs_UserById",
                table: "TransferStockLogs",
                column: "UserById");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferStockLogs_Users_UserById",
                table: "TransferStockLogs",
                column: "UserById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferStockLogs_Users_UserById",
                table: "TransferStockLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransferStockLogs_UserById",
                table: "TransferStockLogs");

            migrationBuilder.DropColumn(
                name: "UserById",
                table: "TransferStockLogs");
        }
    }
}
