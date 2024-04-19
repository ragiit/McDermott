using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditFieldDrugFormMedicament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Form",
                table: "Medicaments");

            migrationBuilder.AddColumn<long>(
                name: "FormId",
                table: "Medicaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_FormId",
                table: "Medicaments",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_FormDrugs_FormId",
                table: "Medicaments",
                column: "FormId",
                principalTable: "FormDrugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_FormDrugs_FormId",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_FormId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Medicaments");

            migrationBuilder.AddColumn<string>(
                name: "Form",
                table: "Medicaments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
