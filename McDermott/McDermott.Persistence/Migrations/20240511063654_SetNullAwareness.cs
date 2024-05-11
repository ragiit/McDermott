using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetNullAwareness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AwarenessId",
                table: "GeneralConsultantClinicalAssesments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AwarnessId",
                table: "GeneralConsultantClinicalAssesments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultantClinicalAssesments_AwarenessId",
                table: "GeneralConsultantClinicalAssesments",
                column: "AwarenessId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_Awarenesses_AwarenessId",
                table: "GeneralConsultantClinicalAssesments",
                column: "AwarenessId",
                principalTable: "Awarenesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_Awarenesses_AwarenessId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultantClinicalAssesments_AwarenessId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "AwarenessId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "AwarnessId",
                table: "GeneralConsultantClinicalAssesments");
        }
    }
}
