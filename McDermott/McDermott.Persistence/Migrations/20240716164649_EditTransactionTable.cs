using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AdjustmentsId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryAdjusmentId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Validate",
                table: "TransactionStocks",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_InventoryAdjusmentId",
                table: "TransactionStocks",
                column: "InventoryAdjusmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_UomId",
                table: "TransactionStocks",
                column: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId",
                table: "TransactionStocks",
                column: "InventoryAdjusmentId",
                principalTable: "InventoryAdjusments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Uoms_UomId",
                table: "TransactionStocks",
                column: "UomId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Uoms_UomId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_UomId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "AdjustmentsId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "Validate",
                table: "TransactionStocks");
        }
    }
}
