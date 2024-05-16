using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SAP",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Oracle",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NIP",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Legacy",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Legacy",
                table: "Users",
                column: "Legacy",
                unique: true,
                filter: "[Legacy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NIP",
                table: "Users",
                column: "NIP",
                unique: true,
                filter: "[NIP] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Oracle",
                table: "Users",
                column: "Oracle",
                unique: true,
                filter: "[Oracle] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SAP",
                table: "Users",
                column: "SAP",
                unique: true,
                filter: "[SAP] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_NoCard",
                table: "InsurancePolicies",
                column: "NoCard",
                unique: true,
                filter: "[NoCard] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Legacy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NIP",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Oracle",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SAP",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_InsurancePolicies_NoCard",
                table: "InsurancePolicies");

            migrationBuilder.AlterColumn<string>(
                name: "SAP",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Oracle",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NIP",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Legacy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
