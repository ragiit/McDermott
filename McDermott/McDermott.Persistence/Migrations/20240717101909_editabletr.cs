using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editabletr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndStock",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "InitialStock",
                table: "TransactionStocks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EndStock",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InitialStock",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);
        }
    }
}
