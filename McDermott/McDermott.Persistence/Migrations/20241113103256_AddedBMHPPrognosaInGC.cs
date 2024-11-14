using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBMHPPrognosaInGC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Anamnesa",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BMHP",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KdPrognosa",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitNumber",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Anamnesa",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationTherapy",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NonMedicationTherapy",
                table: "GeneralConsultanCPPTs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anamnesa",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "BMHP",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "KdPrognosa",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "VisitNumber",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Anamnesa",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "MedicationTherapy",
                table: "GeneralConsultanCPPTs");

            migrationBuilder.DropColumn(
                name: "NonMedicationTherapy",
                table: "GeneralConsultanCPPTs");
        }
    }
}
