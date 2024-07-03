using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
