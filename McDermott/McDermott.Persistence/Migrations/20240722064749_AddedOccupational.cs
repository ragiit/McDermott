using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedOccupational : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OccupationalId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OccupationalId",
                table: "Users",
                column: "OccupationalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Occupationals_OccupationalId",
                table: "Users",
                column: "OccupationalId",
                principalTable: "Occupationals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Occupationals_OccupationalId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OccupationalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OccupationalId",
                table: "Users");
        }
    }
}
