using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class sttsINT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InformationFrom",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "StatusTransfer",
                table: "TransferStockLogs",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TransferStockLogs",
                newName: "StatusTransfer");

            migrationBuilder.AddColumn<string>(
                name: "InformationFrom",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
