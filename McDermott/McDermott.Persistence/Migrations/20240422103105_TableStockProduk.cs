using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TableStockProduk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.AddColumn<bool>(
                name: "TraceAbility",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destinance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusTransaction = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                column: "RegimentOfUseId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_SignaId",
                table: "MedicamentGroupDetails",
                column: "SignaId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_ProductId",
                table: "StockProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                column: "RegimentOfUseId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Products_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Signas_SignaId",
                table: "MedicamentGroupDetails",
                column: "SignaId",
                principalTable: "Signas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Products_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Signas_SignaId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropTable(
                name: "StockProducts");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentGroupDetails_RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentGroupDetails_SignaId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropColumn(
                name: "TraceAbility",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
