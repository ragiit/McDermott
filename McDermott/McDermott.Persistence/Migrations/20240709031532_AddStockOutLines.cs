using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStockOutLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockOutLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinesId = table.Column<long>(type: "bigint", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    CutStock = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOutLines_ConcoctionLines_LinesId",
                        column: x => x.LinesId,
                        principalTable: "ConcoctionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockOutLines_StockProducts_StockId",
                        column: x => x.StockId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockOutLines_LinesId",
                table: "StockOutLines",
                column: "LinesId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutLines_StockId",
                table: "StockOutLines",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockOutLines");
        }
    }
}
