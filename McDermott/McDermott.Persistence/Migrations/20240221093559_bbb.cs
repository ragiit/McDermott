using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class bbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultantServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");
        }
    }
}
