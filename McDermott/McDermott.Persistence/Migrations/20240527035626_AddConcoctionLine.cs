using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConcoctionLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConcoctionLines_ActiveComponents_ActiveComponentId",
                table: "ConcoctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcoctionLines_MedicamentGroups_MedicamentGroupId",
                table: "ConcoctionLines");

            migrationBuilder.DropIndex(
                name: "IX_ConcoctionLines_ActiveComponentId",
                table: "ConcoctionLines");

            migrationBuilder.RenameColumn(
                name: "MedicamentGroupId",
                table: "ConcoctionLines",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ConcoctionLines_MedicamentGroupId",
                table: "ConcoctionLines",
                newName: "IX_ConcoctionLines_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "ActiveComponentId",
                table: "ConcoctionLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConcoctionLineId",
                table: "ActiveComponents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_ConcoctionLineId",
                table: "ActiveComponents",
                column: "ConcoctionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_ConcoctionLines_ConcoctionLineId",
                table: "ActiveComponents",
                column: "ConcoctionLineId",
                principalTable: "ConcoctionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcoctionLines_Products_ProductId",
                table: "ConcoctionLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveComponents_ConcoctionLines_ConcoctionLineId",
                table: "ActiveComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcoctionLines_Products_ProductId",
                table: "ConcoctionLines");

            migrationBuilder.DropIndex(
                name: "IX_ActiveComponents_ConcoctionLineId",
                table: "ActiveComponents");

            migrationBuilder.DropColumn(
                name: "ConcoctionLineId",
                table: "ActiveComponents");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ConcoctionLines",
                newName: "MedicamentGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ConcoctionLines_ProductId",
                table: "ConcoctionLines",
                newName: "IX_ConcoctionLines_MedicamentGroupId");

            migrationBuilder.AlterColumn<long>(
                name: "ActiveComponentId",
                table: "ConcoctionLines",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_ActiveComponentId",
                table: "ConcoctionLines",
                column: "ActiveComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConcoctionLines_ActiveComponents_ActiveComponentId",
                table: "ConcoctionLines",
                column: "ActiveComponentId",
                principalTable: "ActiveComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcoctionLines_MedicamentGroups_MedicamentGroupId",
                table: "ConcoctionLines",
                column: "MedicamentGroupId",
                principalTable: "MedicamentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
