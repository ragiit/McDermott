using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class classtypeQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClassTypeId",
                table: "KioskQueues",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_ClassTypeId",
                table: "KioskQueues",
                column: "ClassTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KioskQueues_ClassTypes_ClassTypeId",
                table: "KioskQueues",
                column: "ClassTypeId",
                principalTable: "ClassTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KioskQueues_ClassTypes_ClassTypeId",
                table: "KioskQueues");

            migrationBuilder.DropIndex(
                name: "IX_KioskQueues_ClassTypeId",
                table: "KioskQueues");

            migrationBuilder.DropColumn(
                name: "ClassTypeId",
                table: "KioskQueues");
        }
    }
}
