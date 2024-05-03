using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTableTransactionStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    QtyStock = table.Column<long>(type: "bigint", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodeTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusTranfer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_StockProducts_StockId",
                        column: x => x.StockId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_DestinationId",
                table: "TransactionStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ProductId",
                table: "TransactionStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_SourceId",
                table: "TransactionStocks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_StockId",
                table: "TransactionStocks",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStocks");
        }
    }
}
