using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Editabletransactionse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdjustmentsId",
                table: "TransactionStocks",
                newName: "SourcTableId");

            migrationBuilder.AddColumn<string>(
                name: "SourceTable",
                table: "TransactionStocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceTable",
                table: "TransactionStocks");

            migrationBuilder.RenameColumn(
                name: "SourcTableId",
                table: "TransactionStocks",
                newName: "AdjustmentsId");
        }
    }
}
