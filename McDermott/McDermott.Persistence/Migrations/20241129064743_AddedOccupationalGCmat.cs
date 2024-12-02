using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedOccupationalGCmat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_JobPositions_JobPositionId",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "JobPositionId",
                table: "GeneralConsultanServices",
                newName: "OccupationalId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultanServices_JobPositionId",
                table: "GeneralConsultanServices",
                newName: "IX_GeneralConsultanServices_OccupationalId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Occupationals_OccupationalId",
                table: "GeneralConsultanServices",
                column: "OccupationalId",
                principalTable: "Occupationals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Occupationals_OccupationalId",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "OccupationalId",
                table: "GeneralConsultanServices",
                newName: "JobPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultanServices_OccupationalId",
                table: "GeneralConsultanServices",
                newName: "IX_GeneralConsultanServices_JobPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_JobPositions_JobPositionId",
                table: "GeneralConsultanServices",
                column: "JobPositionId",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
