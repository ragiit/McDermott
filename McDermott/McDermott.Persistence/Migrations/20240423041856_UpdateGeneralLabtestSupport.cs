using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeneralLabtestSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uom",
                table: "LabResultDetails");

            migrationBuilder.AddColumn<long>(
                name: "LabUomId",
                table: "LabResultDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabResultDetails_LabUomId",
                table: "LabResultDetails",
                column: "LabUomId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_LabUoms_LabUomId",
                table: "LabResultDetails",
                column: "LabUomId",
                principalTable: "LabUoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_LabUoms_LabUomId",
                table: "LabResultDetails");

            migrationBuilder.DropIndex(
                name: "IX_LabResultDetails_LabUomId",
                table: "LabResultDetails");

            migrationBuilder.DropColumn(
                name: "LabUomId",
                table: "LabResultDetails");

            migrationBuilder.AddColumn<string>(
                name: "Uom",
                table: "LabResultDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
