using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editfieldreceiv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusReceived",
                table: "ReceivingStocks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReceivingStocks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReceivingStocks");

            migrationBuilder.AddColumn<string>(
                name: "StatusReceived",
                table: "ReceivingStocks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
