using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addfieldDosageofuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Uoms_UoMId",
                table: "MedicamentGroupDetails");

            migrationBuilder.RenameColumn(
                name: "UoMId",
                table: "MedicamentGroupDetails",
                newName: "UnitOfDosageId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicamentGroupDetails_UoMId",
                table: "MedicamentGroupDetails",
                newName: "IX_MedicamentGroupDetails_UnitOfDosageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Uoms_UnitOfDosageId",
                table: "MedicamentGroupDetails",
                column: "UnitOfDosageId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Uoms_UnitOfDosageId",
                table: "MedicamentGroupDetails");

            migrationBuilder.RenameColumn(
                name: "UnitOfDosageId",
                table: "MedicamentGroupDetails",
                newName: "UoMId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicamentGroupDetails_UnitOfDosageId",
                table: "MedicamentGroupDetails",
                newName: "IX_MedicamentGroupDetails_UoMId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Uoms_UoMId",
                table: "MedicamentGroupDetails",
                column: "UoMId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
