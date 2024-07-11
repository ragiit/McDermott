using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignatureAndConfinedBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignatureEmployeeImagesMedicalHistoryBase64",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureEximinedDoctorBase64",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureEmployeeImagesMedicalHistoryBase64",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "SignatureEximinedDoctorBase64",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
