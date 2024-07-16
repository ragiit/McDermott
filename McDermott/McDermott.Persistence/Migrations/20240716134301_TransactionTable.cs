using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivingStockId",
                table: "TransferStockDetails");

            migrationBuilder.CreateTable(
                name: "TransactionStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingId = table.Column<long>(type: "bigint", nullable: true),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: true),
                    ConcoctionLineId = table.Column<long>(type: "bigint", nullable: true),
                    TransferId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    InitialStock = table.Column<long>(type: "bigint", nullable: true),
                    InStock = table.Column<long>(type: "bigint", nullable: true),
                    OutStock = table.Column<long>(type: "bigint", nullable: true),
                    EndStock = table.Column<long>(type: "bigint", nullable: true),
                    ReceivingStockId = table.Column<long>(type: "bigint", nullable: true),
                    TransferStockId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_ConcoctionLines_ConcoctionLineId",
                        column: x => x.ConcoctionLineId,
                        principalTable: "ConcoctionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_TransactionStocks_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_ReceivingStocks_ReceivingStockId",
                        column: x => x.ReceivingStockId,
                        principalTable: "ReceivingStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_TransferStocks_TransferStockId",
                        column: x => x.TransferStockId,
                        principalTable: "TransferStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ConcoctionLineId",
                table: "TransactionStocks",
                column: "ConcoctionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_DestinationId",
                table: "TransactionStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_PrescriptionId",
                table: "TransactionStocks",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ProductId",
                table: "TransactionStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ReceivingStockId",
                table: "TransactionStocks",
                column: "ReceivingStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_SourceId",
                table: "TransactionStocks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_TransferStockId",
                table: "TransactionStocks",
                column: "TransferStockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStocks");

            migrationBuilder.AddColumn<long>(
                name: "ReceivingStockId",
                table: "TransferStockDetails",
                type: "bigint",
                nullable: true);
        }
    }
}
