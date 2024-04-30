using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditFieldMedicamentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.RenameColumn(
                name: "RegimentOfUseId",
                table: "MedicamentGroupDetails",
                newName: "FrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicamentGroupDetails_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                newName: "IX_MedicamentGroupDetails_FrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_FrequencyId",
                table: "MedicamentGroupDetails",
                column: "FrequencyId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_FrequencyId",
                table: "MedicamentGroupDetails");

            migrationBuilder.RenameColumn(
                name: "FrequencyId",
                table: "MedicamentGroupDetails",
                newName: "RegimentOfUseId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicamentGroupDetails_FrequencyId",
                table: "MedicamentGroupDetails",
                newName: "IX_MedicamentGroupDetails_RegimentOfUseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_DrugDosages_RegimentOfUseId",
                table: "MedicamentGroupDetails",
                column: "RegimentOfUseId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
