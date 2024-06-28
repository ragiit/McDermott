using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReceivingLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusTransfer",
                table: "TransactionStocks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TransactionStocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StockRequest",
                table: "TransactionStocks",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceivingLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_ReceivingStocks_ReceivingId",
                        column: x => x.ReceivingId,
                        principalTable: "ReceivingStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_ReceivingId",
                table: "ReceivingLogs",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_SourceId",
                table: "ReceivingLogs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_UserById",
                table: "ReceivingLogs",
                column: "UserById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivingLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "StockRequest",
                table: "TransactionStocks");

            migrationBuilder.AddColumn<string>(
                name: "StatusTransfer",
                table: "TransactionStocks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
