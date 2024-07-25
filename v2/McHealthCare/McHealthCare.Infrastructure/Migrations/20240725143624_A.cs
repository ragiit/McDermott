using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Province_Countries_CountryId",
                table: "Province");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Province",
                table: "Province");

            migrationBuilder.RenameTable(
                name: "Province",
                newName: "Provinces");

            migrationBuilder.RenameIndex(
                name: "IX_Province_CountryId",
                table: "Provinces",
                newName: "IX_Provinces_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Countries_CountryId",
                table: "Provinces",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Countries_CountryId",
                table: "Provinces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces");

            migrationBuilder.RenameTable(
                name: "Provinces",
                newName: "Province");

            migrationBuilder.RenameIndex(
                name: "IX_Provinces_CountryId",
                table: "Province",
                newName: "IX_Province_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Province",
                table: "Province",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Province_Countries_CountryId",
                table: "Province",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
