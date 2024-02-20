using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editFieldKiosk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageInsurance",
                table: "Kiosks");

            migrationBuilder.RenameColumn(
                name: "Insurance",
                table: "Kiosks",
                newName: "BPJS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BPJS",
                table: "Kiosks",
                newName: "Insurance");

            migrationBuilder.AddColumn<bool>(
                name: "StageInsurance",
                table: "Kiosks",
                type: "bit",
                nullable: true);
        }
    }
}
