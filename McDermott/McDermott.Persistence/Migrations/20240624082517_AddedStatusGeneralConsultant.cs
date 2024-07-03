using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusGeneralConsultant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StagingStatus",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GeneralConsultanServices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<string>(
                name: "StagingStatus",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
