using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTableKioskQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kiosks_Counters_CounterId",
                table: "Kiosks");

            migrationBuilder.DropIndex(
                name: "IX_Kiosks_CounterId",
                table: "Kiosks");

            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "Kiosks");

            migrationBuilder.DropColumn(
                name: "Queue",
                table: "Kiosks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CounterId",
                table: "Kiosks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Queue",
                table: "Kiosks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kiosks_CounterId",
                table: "Kiosks",
                column: "CounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosks_Counters_CounterId",
                table: "Kiosks",
                column: "CounterId",
                principalTable: "Counters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}