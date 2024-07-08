using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSickleavesCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId",
                table: "SickLeaves");

            migrationBuilder.AddForeignKey(
                name: "FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId",
                table: "SickLeaves",
                column: "GeneralConsultansId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId",
                table: "SickLeaves");

            migrationBuilder.AddForeignKey(
                name: "FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId",
                table: "SickLeaves",
                column: "GeneralConsultansId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
