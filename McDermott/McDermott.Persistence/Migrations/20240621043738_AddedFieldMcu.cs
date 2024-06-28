using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldMcu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBatam",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMcu",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutsideBatam",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedexType",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBatam",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsMcu",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsOutsideBatam",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "MedexType",
                table: "GeneralConsultanServices");
        }
    }
}
