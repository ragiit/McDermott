using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableGeneralSupportAndProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupraventricularExtraSystole",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsVentriculatExtraSystole",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.RenameColumn(
                name: "RBS",
                table: "GeneralConsultantClinicalAssesments",
                newName: "PainScale");

            migrationBuilder.AddColumn<long>(
                name: "HR",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HR",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.RenameColumn(
                name: "PainScale",
                table: "GeneralConsultantClinicalAssesments",
                newName: "RBS");

            migrationBuilder.AddColumn<bool>(
                name: "IsSupraventricularExtraSystole",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVentriculatExtraSystole",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
