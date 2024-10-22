using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedProjectField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "Accidents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_ProjectId",
                table: "Accidents",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Projects_ProjectId",
                table: "Accidents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Projects_ProjectId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_ProjectId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Accidents");
        }
    }
}
