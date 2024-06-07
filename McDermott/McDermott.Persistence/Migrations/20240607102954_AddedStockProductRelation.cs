using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedStockProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StockProductId",
                table: "TransactionStockProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockProduct_StockProductId",
                table: "TransactionStockProduct",
                column: "StockProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStockProduct_StockProducts_StockProductId",
                table: "TransactionStockProduct",
                column: "StockProductId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStockProduct_StockProducts_StockProductId",
                table: "TransactionStockProduct");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStockProduct_StockProductId",
                table: "TransactionStockProduct");

            migrationBuilder.DropColumn(
                name: "StockProductId",
                table: "TransactionStockProduct");
        }
    }
}
