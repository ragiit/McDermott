using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFieldStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destinance",
                table: "StockProducts");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "StockProducts",
                newName: "SerialNumber");

            migrationBuilder.AddColumn<long>(
                name: "DestinanceId",
                table: "StockProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceId",
                table: "StockProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "StockProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_DestinanceId",
                table: "StockProducts",
                column: "DestinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_SourceId",
                table: "StockProducts",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_UomId",
                table: "StockProducts",
                column: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockProducts_Locations_DestinanceId",
                table: "StockProducts",
                column: "DestinanceId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockProducts_Locations_SourceId",
                table: "StockProducts",
                column: "SourceId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockProducts_Uoms_UomId",
                table: "StockProducts",
                column: "UomId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockProducts_Locations_DestinanceId",
                table: "StockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockProducts_Locations_SourceId",
                table: "StockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockProducts_Uoms_UomId",
                table: "StockProducts");

            migrationBuilder.DropIndex(
                name: "IX_StockProducts_DestinanceId",
                table: "StockProducts");

            migrationBuilder.DropIndex(
                name: "IX_StockProducts_SourceId",
                table: "StockProducts");

            migrationBuilder.DropIndex(
                name: "IX_StockProducts_UomId",
                table: "StockProducts");

            migrationBuilder.DropColumn(
                name: "DestinanceId",
                table: "StockProducts");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "StockProducts");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "StockProducts");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "StockProducts",
                newName: "Source");

            migrationBuilder.AddColumn<string>(
                name: "Destinance",
                table: "StockProducts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
