using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTypeIdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassType",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<long>(
                name: "ClassTypeId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ClassTypeId",
                table: "GeneralConsultanServices",
                column: "ClassTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_ClassTypes_ClassTypeId",
                table: "GeneralConsultanServices",
                column: "ClassTypeId",
                principalTable: "ClassTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_ClassTypes_ClassTypeId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_ClassTypeId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ClassTypeId",
                table: "GeneralConsultanServices");

            migrationBuilder.AddColumn<string>(
                name: "ClassType",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}