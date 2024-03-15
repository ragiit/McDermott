using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFIeldInMedicalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PractitionerECGId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "GeneralConsultanMedicalSupports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerAlcoholEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerDrugEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerECGId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerECGId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerLabEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerRadiologyEximinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerAlcoholEximinationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerDrugEximinationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerECGId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerECGId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerLabEximinationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerRadiologyEximinationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerECGId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerECGId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "PractitionerECGId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeneralConsultanMedicalSupports");
        }
    }
}
