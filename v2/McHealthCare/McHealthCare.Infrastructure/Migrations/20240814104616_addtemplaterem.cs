using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addtemplaterem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_AspNetUsers_ById1",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_ById1",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "ById1",
                table: "EmailTemplates");

            migrationBuilder.AlterColumn<string>(
                name: "ToPartnerId",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ById",
                table: "EmailTemplates",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ById",
                table: "EmailTemplates",
                column: "ById");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_AspNetUsers_ById",
                table: "EmailTemplates",
                column: "ById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_AspNetUsers_ById",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_ById",
                table: "EmailTemplates");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToPartnerId",
                table: "EmailTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ById",
                table: "EmailTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ById1",
                table: "EmailTemplates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ById1",
                table: "EmailTemplates",
                column: "ById1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_AspNetUsers_ById1",
                table: "EmailTemplates",
                column: "ById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
