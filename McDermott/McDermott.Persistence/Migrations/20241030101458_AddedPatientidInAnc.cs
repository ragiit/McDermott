using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPatientidInAnc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServiceAncs_PatientId",
                table: "GeneralConsultanServiceAncs",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServiceAncs_Users_PatientId",
                table: "GeneralConsultanServiceAncs",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServiceAncs_Users_PatientId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServiceAncs_PatientId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "GeneralConsultanServiceAncs");
        }
    }
}
