using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedTableNameUom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "ActiveComponents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Uoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UomCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiggerRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoundingPrecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uoms_UomCategories_UomCategoryId",
                        column: x => x.UomCategoryId,
                        principalTable: "UomCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_UomId",
                table: "ActiveComponents",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_Uoms_UomCategoryId",
                table: "Uoms",
                column: "UomCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_Uoms_UomId",
                table: "ActiveComponents",
                column: "UomId",
                principalTable: "Uoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveComponents_Uoms_UomId",
                table: "ActiveComponents");

            migrationBuilder.DropTable(
                name: "Uoms");

            migrationBuilder.DropIndex(
                name: "IX_ActiveComponents_UomId",
                table: "ActiveComponents");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "ActiveComponents");
        }
    }
}
