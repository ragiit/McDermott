using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_LabTestDetails_LabTestId",
                table: "LabResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropIndex(
                name: "IX_LabResultDetails_LabTestId",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "LabTestId",
                table: "LabResultDetails");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalRange",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parameter",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uom",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                column: "RegimentOfUseId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_SignaId",
                table: "MedicamentGroupDetails",
                column: "SignaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                column: "RegimentOfUseId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Products_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Signas_SignaId",
                table: "MedicamentGroupDetails",
                column: "SignaId",
                principalTable: "Signas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Products_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Signas_SignaId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentGroupDetails_RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentGroupDetails_SignaId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "NormalRange",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "Parameter",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "Uom",
                table: "LabResultDetails");

            migrationBuilder.AddColumn<long>(
                name: "LabTestId",
                table: "LabResultDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabResultDetails_LabTestId",
                table: "LabResultDetails",
                column: "LabTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_LabTestDetails_LabTestId",
                table: "LabResultDetails",
                column: "LabTestId",
                principalTable: "LabTestDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
