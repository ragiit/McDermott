using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editTableGeneralConsultanServicesd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PratitionerId",
                table: "GeneralConsultanServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_PratitionerId",
                table: "GeneralConsultanServices",
                column: "PratitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Users_PratitionerId",
                table: "GeneralConsultanServices",
                column: "PratitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Users_PratitionerId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_PratitionerId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "PratitionerId",
                table: "GeneralConsultanServices");
        }
    }
}
