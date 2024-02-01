using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTableEmailTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    from = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ById = table.Column<int>(type: "int", nullable: true),
                    To = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ToPartnerId = table.Column<int>(type: "int", nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReplayTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Schendule = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Users_ById",
                        column: x => x.ById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailTemplateId",
                table: "Users",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ById",
                table: "EmailTemplates",
                column: "ById");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_EmailTemplates_EmailTemplateId",
                table: "Users",
                column: "EmailTemplateId",
                principalTable: "EmailTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_EmailTemplates_EmailTemplateId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmailTemplateId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailTemplateId",
                table: "Users");
        }
    }
}
