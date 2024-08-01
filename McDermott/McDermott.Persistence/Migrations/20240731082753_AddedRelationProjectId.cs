using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationProjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ProjectId",
                table: "GeneralConsultanServices",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Projects_ProjectId",
                table: "GeneralConsultanServices",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Projects_ProjectId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_ProjectId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "GeneralConsultanServices");
        }
    }
}
