using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldNumberPurchaseReceiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberPurchase",
                table: "ReceivingStocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberPurchase",
                table: "ReceivingStocks");
        }
    }
}
