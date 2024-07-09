using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSafetyPersonnelName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SafetyPersonnelId",
                table: "Accidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_SafetyPersonnelId",
                table: "Accidents",
                column: "SafetyPersonnelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Users_SafetyPersonnelId",
                table: "Accidents",
                column: "SafetyPersonnelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Users_SafetyPersonnelId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_SafetyPersonnelId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "SafetyPersonnelId",
                table: "Accidents");
        }
    }
}
