using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOutLines_StockProducts_StockId",
                table: "StockOutLines");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOutPrescriptions_StockProducts_StockId",
                table: "StockOutPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Locations_DestinationId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Locations_SourceId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_DestinationId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "TransactionStocks");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "TransactionStocks",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionStocks_SourceId",
                table: "TransactionStocks",
                newName: "IX_TransactionStocks_LocationId");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "StockOutPrescriptions",
                newName: "TransactionStockId");

            migrationBuilder.RenameIndex(
                name: "IX_StockOutPrescriptions_StockId",
                table: "StockOutPrescriptions",
                newName: "IX_StockOutPrescriptions_TransactionStockId");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "StockOutLines",
                newName: "TransactionStockId");

            migrationBuilder.RenameIndex(
                name: "IX_StockOutLines_StockId",
                table: "StockOutLines",
                newName: "IX_StockOutLines_TransactionStockId");

            migrationBuilder.AddColumn<long>(
                name: "StockProductId",
                table: "StockOutPrescriptions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockOutPrescriptions_StockProductId",
                table: "StockOutPrescriptions",
                column: "StockProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOutLines_TransactionStocks_TransactionStockId",
                table: "StockOutLines",
                column: "TransactionStockId",
                principalTable: "TransactionStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOutPrescriptions_StockProducts_StockProductId",
                table: "StockOutPrescriptions",
                column: "StockProductId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOutPrescriptions_TransactionStocks_TransactionStockId",
                table: "StockOutPrescriptions",
                column: "TransactionStockId",
                principalTable: "TransactionStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Locations_LocationId",
                table: "TransactionStocks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOutLines_TransactionStocks_TransactionStockId",
                table: "StockOutLines");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOutPrescriptions_StockProducts_StockProductId",
                table: "StockOutPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOutPrescriptions_TransactionStocks_TransactionStockId",
                table: "StockOutPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Locations_LocationId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_StockOutPrescriptions_StockProductId",
                table: "StockOutPrescriptions");

            migrationBuilder.DropColumn(
                name: "StockProductId",
                table: "StockOutPrescriptions");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "TransactionStocks",
                newName: "SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionStocks_LocationId",
                table: "TransactionStocks",
                newName: "IX_TransactionStocks_SourceId");

            migrationBuilder.RenameColumn(
                name: "TransactionStockId",
                table: "StockOutPrescriptions",
                newName: "StockId");

            migrationBuilder.RenameIndex(
                name: "IX_StockOutPrescriptions_TransactionStockId",
                table: "StockOutPrescriptions",
                newName: "IX_StockOutPrescriptions_StockId");

            migrationBuilder.RenameColumn(
                name: "TransactionStockId",
                table: "StockOutLines",
                newName: "StockId");

            migrationBuilder.RenameIndex(
                name: "IX_StockOutLines_TransactionStockId",
                table: "StockOutLines",
                newName: "IX_StockOutLines_StockId");

            migrationBuilder.AddColumn<long>(
                name: "DestinationId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_DestinationId",
                table: "TransactionStocks",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOutLines_StockProducts_StockId",
                table: "StockOutLines",
                column: "StockId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOutPrescriptions_StockProducts_StockId",
                table: "StockOutPrescriptions",
                column: "StockId",
                principalTable: "StockProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Locations_DestinationId",
                table: "TransactionStocks",
                column: "DestinationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Locations_SourceId",
                table: "TransactionStocks",
                column: "SourceId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
