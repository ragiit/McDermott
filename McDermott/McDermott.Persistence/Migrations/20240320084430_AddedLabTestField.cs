using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedLabTestField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabResulLabExaminationtId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabResulLabExaminationtId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
