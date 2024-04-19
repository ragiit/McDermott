using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentExamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ECGAttachment",
                table: "GeneralConsultanMedicalSupports",
                newName: "OtherExaminationAttachment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherExaminationAttachment",
                table: "GeneralConsultanMedicalSupports",
                newName: "ECGAttachment");
        }
    }
}
