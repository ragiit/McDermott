using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditFieldServiceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.RenameColumn(
                name: "GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                newName: "GeneralConsultanServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                newName: "IX_GeneralConsultantClinicalAssesments_GeneralConsultanServiceId");

            migrationBuilder.AlterColumn<int>(
                name: "ServicedId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServicedId",
                table: "Services",
                column: "ServicedId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services",
                column: "ServicedId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServicedId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                newName: "GeneralConsultantServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                newName: "IX_GeneralConsultantClinicalAssesments_GeneralConsultantServiceId");

            migrationBuilder.AlterColumn<string>(
                name: "ServicedId",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultantServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultantServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
