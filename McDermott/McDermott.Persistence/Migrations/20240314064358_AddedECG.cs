using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedECG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNormalRestingECG",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOtherECG",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSinusBradycardia",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSinusRhythm",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSinusTachycardia",
                table: "GeneralConsultanMedicalSupports",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<string>(
                name: "OtherDesc",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNormalRestingECG",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsOtherECG",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsSinusBradycardia",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsSinusRhythm",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsSinusTachycardia",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsSupraventricularExtraSystole",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "IsVentriculatExtraSystole",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "OtherDesc",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}