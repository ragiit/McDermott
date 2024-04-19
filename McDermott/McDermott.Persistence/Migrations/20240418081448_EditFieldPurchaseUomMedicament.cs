using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditFieldPurchaseUomMedicament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseUom",
                table: "GeneralInformations");

            migrationBuilder.AddColumn<long>(
                name: "PurchaseUomId",
                table: "GeneralInformations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_PurchaseUomId",
                table: "GeneralInformations",
                column: "PurchaseUomId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInformations_Uoms_PurchaseUomId",
                table: "GeneralInformations",
                column: "PurchaseUomId",
                principalTable: "Uoms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInformations_Uoms_PurchaseUomId",
                table: "GeneralInformations");

            migrationBuilder.DropIndex(
                name: "IX_GeneralInformations_PurchaseUomId",
                table: "GeneralInformations");

            migrationBuilder.DropColumn(
                name: "PurchaseUomId",
                table: "GeneralInformations");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseUom",
                table: "GeneralInformations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
