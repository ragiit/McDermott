using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ClaimGCs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GeneralConsultanServiceId",
                table: "ClaimRequests",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimRequests_GeneralConsultanServiceId",
                table: "ClaimRequests",
                column: "GeneralConsultanServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimRequests_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "ClaimRequests",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimRequests_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "ClaimRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClaimRequests_GeneralConsultanServiceId",
                table: "ClaimRequests");

            migrationBuilder.DropColumn(
                name: "GeneralConsultanServiceId",
                table: "ClaimRequests");
        }
    }
}
