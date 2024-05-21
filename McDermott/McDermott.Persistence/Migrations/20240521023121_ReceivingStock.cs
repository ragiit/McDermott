using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReceivingStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStockProduct_StockProducts_StockId",
                table: "TransactionStockProduct");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStockProduct_StockId",
                table: "TransactionStockProduct");

            migrationBuilder.DropColumn(
                name: "StatusStock",
                table: "TransactionStockProduct");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "TransactionStockProduct");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "ReceivingStockDetails");

            migrationBuilder.DropColumn(
                name: "SchenduleDate",
                table: "ReceivingStockDetails");

            migrationBuilder.DropColumn(
                name: "StatusReceived",
                table: "ReceivingStockDetails");

            migrationBuilder.AddColumn<long>(
                name: "Qty",
                table: "ReceivingStockDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceivingStockId",
                table: "ReceivingStockDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceivingStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusReceived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingStocks_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStockDetails_ReceivingStockId",
                table: "ReceivingStockDetails",
                column: "ReceivingStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStocks_DestinationId",
                table: "ReceivingStocks",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingStockDetails_ReceivingStocks_ReceivingStockId",
                table: "ReceivingStockDetails",
                column: "ReceivingStockId",
                principalTable: "ReceivingStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingStockDetails_ReceivingStocks_ReceivingStockId",
                table: "ReceivingStockDetails");

            migrationBuilder.DropTable(
                name: "ReceivingStocks");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingStockDetails_ReceivingStockId",
                table: "ReceivingStockDetails");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "ReceivingStockDetails");

            migrationBuilder.DropColumn(
                name: "ReceivingStockId",
                table: "ReceivingStockDetails");

            migrationBuilder.AddColumn<string>(
                name: "StatusStock",
                table: "TransactionStockProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "TransactionStockProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "ReceivingStockDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SchenduleDate",
                table: "ReceivingStockDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusReceived",
                table: "ReceivingStockDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStockProduct_StockId",
                table: "TransactionStockProduct",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStockProduct_StockProducts_StockId",
                table: "TransactionStockProduct",
                column: "StockId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
