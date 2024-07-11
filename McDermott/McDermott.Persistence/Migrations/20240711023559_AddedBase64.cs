using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfinedSpace",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.AddColumn<string>(
                name: "McuExaminationBase64",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "McuExaminationDocs",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "McuExaminationBase64",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "McuExaminationDocs",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfinedSpace",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
