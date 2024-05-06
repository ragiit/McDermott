using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTransactionStockDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Products_ProductId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_StockProducts_StockId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_ProductId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "QtyStock",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "StatusStock",
                table: "TransactionStocks");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "TransactionStocks",
                newName: "StockProductId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionStocks_StockId",
                table: "TransactionStocks",
                newName: "IX_TransactionStocks_StockProductId");

            migrationBuilder.CreateTable(
                name: "TransactionStockDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    QtyStock = table.Column<long>(type: "bigint", nullable: true),
                    StatusStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStockDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStockDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStockDetail_StockProducts_StockId",
                        column: x => x.StockId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStockDetail_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetail_ProductId",
                table: "TransactionStockDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetail_StockId",
                table: "TransactionStockDetail",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetail_TransactionStockId",
                table: "TransactionStockDetail",
                column: "TransactionStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_StockProducts_StockProductId",
                table: "TransactionStocks",
                column: "StockProductId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_StockProducts_StockProductId",
                table: "TransactionStocks");

            migrationBuilder.DropTable(
                name: "TransactionStockDetail");

            migrationBuilder.RenameColumn(
                name: "StockProductId",
                table: "TransactionStocks",
                newName: "StockId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionStocks_StockProductId",
                table: "TransactionStocks",
                newName: "IX_TransactionStocks_StockId");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QtyStock",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusStock",
                table: "TransactionStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ProductId",
                table: "TransactionStocks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Products_ProductId",
                table: "TransactionStocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_StockProducts_StockId",
                table: "TransactionStocks",
                column: "StockId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
