using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editfieldConcoction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concoctions_Uoms_UomId",
                table: "Concoctions");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "Concoctions");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Concoctions");

            migrationBuilder.RenameColumn(
                name: "UomId",
                table: "Concoctions",
                newName: "DrugRouteId");

            migrationBuilder.RenameColumn(
                name: "TotalQty",
                table: "Concoctions",
                newName: "DrugDosageId");

            migrationBuilder.RenameColumn(
                name: "QtyByDay",
                table: "Concoctions",
                newName: "ConcoctionQty");

            migrationBuilder.RenameIndex(
                name: "IX_Concoctions_UomId",
                table: "Concoctions",
                newName: "IX_Concoctions_DrugRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_DrugDosageId",
                table: "Concoctions",
                column: "DrugDosageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_DrugDosages_DrugDosageId",
                table: "Concoctions",
                column: "DrugDosageId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_DrugRoutes_DrugRouteId",
                table: "Concoctions",
                column: "DrugRouteId",
                principalTable: "DrugRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concoctions_DrugDosages_DrugDosageId",
                table: "Concoctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Concoctions_DrugRoutes_DrugRouteId",
                table: "Concoctions");

            migrationBuilder.DropIndex(
                name: "IX_Concoctions_DrugDosageId",
                table: "Concoctions");

            migrationBuilder.RenameColumn(
                name: "DrugRouteId",
                table: "Concoctions",
                newName: "UomId");

            migrationBuilder.RenameColumn(
                name: "DrugDosageId",
                table: "Concoctions",
                newName: "TotalQty");

            migrationBuilder.RenameColumn(
                name: "ConcoctionQty",
                table: "Concoctions",
                newName: "QtyByDay");

            migrationBuilder.RenameIndex(
                name: "IX_Concoctions_DrugRouteId",
                table: "Concoctions",
                newName: "IX_Concoctions_UomId");

            migrationBuilder.AddColumn<long>(
                name: "Day",
                table: "Concoctions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Qty",
                table: "Concoctions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_Uoms_UomId",
                table: "Concoctions",
                column: "UomId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
