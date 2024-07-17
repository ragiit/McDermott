using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editableTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingStockId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferStockId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_ReceivingStockId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_TransferStockId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "ReceivingStockId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "TransferStockId",
                table: "TransactionStocks");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ReceivingId",
                table: "TransactionStocks",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_TransferId",
                table: "TransactionStocks",
                column: "TransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingId",
                table: "TransactionStocks",
                column: "ReceivingId",
                principalTable: "ReceivingStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferId",
                table: "TransactionStocks",
                column: "TransferId",
                principalTable: "TransferStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_ReceivingId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_TransferId",
                table: "TransactionStocks");

            migrationBuilder.AddColumn<long>(
                name: "ReceivingStockId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransferStockId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ReceivingStockId",
                table: "TransactionStocks",
                column: "ReceivingStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_TransferStockId",
                table: "TransactionStocks",
                column: "TransferStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingStockId",
                table: "TransactionStocks",
                column: "ReceivingStockId",
                principalTable: "ReceivingStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferStockId",
                table: "TransactionStocks",
                column: "TransferStockId",
                principalTable: "TransferStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
