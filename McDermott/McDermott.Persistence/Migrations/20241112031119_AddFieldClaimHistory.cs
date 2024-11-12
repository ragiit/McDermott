using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldClaimHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PhycisianId",
                table: "ClaimHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PyhicisianId",
                table: "ClaimHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimHistories_PhycisianId",
                table: "ClaimHistories",
                column: "PhycisianId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimHistories_Users_PhycisianId",
                table: "ClaimHistories",
                column: "PhycisianId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimHistories_Users_PhycisianId",
                table: "ClaimHistories");

            migrationBuilder.DropIndex(
                name: "IX_ClaimHistories_PhycisianId",
                table: "ClaimHistories");

            migrationBuilder.DropColumn(
                name: "PhycisianId",
                table: "ClaimHistories");

            migrationBuilder.DropColumn(
                name: "PyhicisianId",
                table: "ClaimHistories");
        }
    }
}
