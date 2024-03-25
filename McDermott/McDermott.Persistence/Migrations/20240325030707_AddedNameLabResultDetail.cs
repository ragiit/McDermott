using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameLabResultDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetail_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                table: "LabResultDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetail_LabTests_LabTestId",
                table: "LabResultDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabResultDetail",
                table: "LabResultDetail");

            migrationBuilder.RenameTable(
                name: "LabResultDetail",
                newName: "LabResultDetails");

            migrationBuilder.RenameColumn(
                name: "ResultValueType",
                table: "LabResultDetails",
                newName: "ResultType");

            migrationBuilder.RenameIndex(
                name: "IX_LabResultDetail_LabTestId",
                table: "LabResultDetails",
                newName: "IX_LabResultDetails_LabTestId");

            migrationBuilder.RenameIndex(
                name: "IX_LabResultDetail_GeneralConsultanMedicalSupportId",
                table: "LabResultDetails",
                newName: "IX_LabResultDetails_GeneralConsultanMedicalSupportId");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabResultDetails",
                table: "LabResultDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                table: "LabResultDetails",
                column: "GeneralConsultanMedicalSupportId",
                principalTable: "GeneralConsultanMedicalSupports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_LabTests_LabTestId",
                table: "LabResultDetails",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                table: "LabResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_LabTests_LabTestId",
                table: "LabResultDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabResultDetails",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "LabResultDetails");

            migrationBuilder.RenameTable(
                name: "LabResultDetails",
                newName: "LabResultDetail");

            migrationBuilder.RenameColumn(
                name: "ResultType",
                table: "LabResultDetail",
                newName: "ResultValueType");

            migrationBuilder.RenameIndex(
                name: "IX_LabResultDetails_LabTestId",
                table: "LabResultDetail",
                newName: "IX_LabResultDetail_LabTestId");

            migrationBuilder.RenameIndex(
                name: "IX_LabResultDetails_GeneralConsultanMedicalSupportId",
                table: "LabResultDetail",
                newName: "IX_LabResultDetail_GeneralConsultanMedicalSupportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabResultDetail",
                table: "LabResultDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetail_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                table: "LabResultDetail",
                column: "GeneralConsultanMedicalSupportId",
                principalTable: "GeneralConsultanMedicalSupports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetail_LabTests_LabTestId",
                table: "LabResultDetail",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
