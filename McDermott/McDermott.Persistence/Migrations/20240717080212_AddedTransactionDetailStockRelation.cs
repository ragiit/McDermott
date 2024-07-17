using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedTransactionDetailStockRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TransactionStockId",
                table: "InventoryAdjusmentDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_TransactionStockId",
                table: "InventoryAdjusmentDetails",
                column: "TransactionStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAdjusmentDetails_TransactionStocks_TransactionStockId",
                table: "InventoryAdjusmentDetails",
                column: "TransactionStockId",
                principalTable: "TransactionStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAdjusmentDetails_TransactionStocks_TransactionStockId",
                table: "InventoryAdjusmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAdjusmentDetails_TransactionStockId",
                table: "InventoryAdjusmentDetails");

            migrationBuilder.DropColumn(
                name: "TransactionStockId",
                table: "InventoryAdjusmentDetails");
        }
    }
}
