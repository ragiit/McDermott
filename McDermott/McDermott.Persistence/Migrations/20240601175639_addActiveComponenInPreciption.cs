using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addActiveComponenInPreciption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrugRoutName",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Prescriptions",
                newName: "ActiveComponentId");

            migrationBuilder.AddColumn<long>(
                name: "PrescriptionId",
                table: "ActiveComponents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_PrescriptionId",
                table: "ActiveComponents",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_Prescriptions_PrescriptionId",
                table: "ActiveComponents",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveComponents_Prescriptions_PrescriptionId",
                table: "ActiveComponents");

            migrationBuilder.DropIndex(
                name: "IX_ActiveComponents_PrescriptionId",
                table: "ActiveComponents");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "ActiveComponents");

            migrationBuilder.RenameColumn(
                name: "ActiveComponentId",
                table: "Prescriptions",
                newName: "ProductName");

            migrationBuilder.AddColumn<string>(
                name: "DrugRoutName",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
