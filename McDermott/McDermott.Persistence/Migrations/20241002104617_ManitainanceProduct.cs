using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ManitainanceProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintainances_Products_EquipmentId",
                table: "Maintainances");

            migrationBuilder.DropIndex(
                name: "IX_Maintainances_EquipmentId",
                table: "Maintainances");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Maintainances");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Maintainances");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Maintainances");

            migrationBuilder.CreateTable(
                name: "MaintainanceProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintainanceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintainanceProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintainanceProducts_Maintainances_MaintainanceId",
                        column: x => x.MaintainanceId,
                        principalTable: "Maintainances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintainanceProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintainanceProducts_MaintainanceId",
                table: "MaintainanceProducts",
                column: "MaintainanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainanceProducts_ProductId",
                table: "MaintainanceProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintainanceProducts");

            migrationBuilder.AddColumn<long>(
                name: "EquipmentId",
                table: "Maintainances",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Maintainances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Maintainances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_EquipmentId",
                table: "Maintainances",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintainances_Products_EquipmentId",
                table: "Maintainances",
                column: "EquipmentId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
