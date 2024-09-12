using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editEmailcctp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailSettings_EmailTemplates_EmailTemplateId",
                table: "EmailSettings");

            migrationBuilder.DropIndex(
                name: "IX_EmailSettings_EmailTemplateId",
                table: "EmailSettings");

            migrationBuilder.DropColumn(
                name: "EmailTemplateId",
                table: "EmailSettings");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_EmailFromId",
                table: "EmailTemplates",
                column: "EmailFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_EmailSettings_EmailFromId",
                table: "EmailTemplates",
                column: "EmailFromId",
                principalTable: "EmailSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_EmailSettings_EmailFromId",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_EmailFromId",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<long>(
                name: "EmailTemplateId",
                table: "EmailSettings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailSettings_EmailTemplateId",
                table: "EmailSettings",
                column: "EmailTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSettings_EmailTemplates_EmailTemplateId",
                table: "EmailSettings",
                column: "EmailTemplateId",
                principalTable: "EmailTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
