using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedStockProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StockProductId",
                table: "InventoryAdjusmentDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_StockProductId",
                table: "InventoryAdjusmentDetails",
                column: "StockProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAdjusmentDetails_StockProducts_StockProductId",
                table: "InventoryAdjusmentDetails",
                column: "StockProductId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAdjusmentDetails_StockProducts_StockProductId",
                table: "InventoryAdjusmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAdjusmentDetails_StockProductId",
                table: "InventoryAdjusmentDetails");

            migrationBuilder.DropColumn(
                name: "StockProductId",
                table: "InventoryAdjusmentDetails");
        }
    }
}
