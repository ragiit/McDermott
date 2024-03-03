using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultantServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultantServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}