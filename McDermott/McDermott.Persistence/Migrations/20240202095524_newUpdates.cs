using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class newUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdmissionQueue",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GeneralConsultanServices",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionQueue",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeneralConsultanServices");
        }
    }
}
