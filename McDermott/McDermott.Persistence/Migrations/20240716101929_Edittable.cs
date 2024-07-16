using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Edittable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStockDetails");

            migrationBuilder.DropTable(
                name: "TransactionStockProduct");

            migrationBuilder.DropTable(
                name: "TransactionStocks");

            migrationBuilder.CreateTable(
                name: "TransferStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodeTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockRequest = table.Column<bool>(type: "bit", nullable: true),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStocks_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStocks_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStocks_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferStockDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferStockId = table.Column<long>(type: "bigint", nullable: true),
                    ReceivingStockId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    StatusTransfer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStockDetails_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransferStockDetails_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransferStockDetails_TransferStocks_TransferStockId",
                        column: x => x.TransferStockId,
                        principalTable: "TransferStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransferStockProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    TransferStockId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    QtyStock = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStockProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStockProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStockProduct_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStockProduct_TransferStocks_TransferStockId",
                        column: x => x.TransferStockId,
                        principalTable: "TransferStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockDetails_DestinationId",
                table: "TransferStockDetails",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockDetails_SourceId",
                table: "TransferStockDetails",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockDetails_TransferStockId",
                table: "TransferStockDetails",
                column: "TransferStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockProduct_ProductId",
                table: "TransferStockProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockProduct_StockProductId",
                table: "TransferStockProduct",
                column: "StockProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockProduct_TransferStockId",
                table: "TransferStockProduct",
                column: "TransferStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_DestinationId",
                table: "TransferStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_SourceId",
                table: "TransferStocks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_StockProductId",
                table: "TransferStocks",
                column: "StockProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferStockDetails");

            migrationBuilder.DropTable(
                name: "TransferStockProduct");

            migrationBuilder.DropTable(
                name: "TransferStocks");

            migrationBuilder.CreateTable(
                name: "TransactionStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodeTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    StockRequest = table.Column<bool>(type: "bit", nullable: true),
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
                        name: "FK_TransactionStocks_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStockDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivingStockId = table.Column<long>(type: "bigint", nullable: true),
                    StatusTransfer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeTransaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStockDetails_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionStockDetails_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransactionStockDetails_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStockProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QtyStock = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStockProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStockProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStockProduct_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStockProduct_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetails_DestinationId",
                table: "TransactionStockDetails",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetails_SourceId",
                table: "TransactionStockDetails",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockDetails_TransactionStockId",
                table: "TransactionStockDetails",
                column: "TransactionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockProduct_ProductId",
                table: "TransactionStockProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockProduct_StockProductId",
                table: "TransactionStockProduct",
                column: "StockProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockProduct_TransactionStockId",
                table: "TransactionStockProduct",
                column: "TransactionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_DestinationId",
                table: "TransactionStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_SourceId",
                table: "TransactionStocks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_StockProductId",
                table: "TransactionStocks",
                column: "StockProductId");
        }
    }
}
