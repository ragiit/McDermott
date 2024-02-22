using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditFieldServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServicedId",
                table: "Services");

            migrationBuilder.AlterColumn<string>(
                name: "ServicedId",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicedId1",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServicedId1",
                table: "Services",
                column: "ServicedId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Services_ServicedId1",
                table: "Services",
                column: "ServicedId1",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Services_ServicedId1",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServicedId1",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServicedId1",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ServicedId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServicedId",
                table: "Services",
                column: "ServicedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services",
                column: "ServicedId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
