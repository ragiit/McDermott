using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccidentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "InventoryAdjusmentId",
                table: "TransactionStocks");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageToBase64",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "ImageToBase64",
                table: "Accidents");

            migrationBuilder.AddColumn<long>(
                name: "InventoryAdjusmentId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_InventoryAdjusmentId",
                table: "TransactionStocks",
                column: "InventoryAdjusmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId",
                table: "TransactionStocks",
                column: "InventoryAdjusmentId",
                principalTable: "InventoryAdjusments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
