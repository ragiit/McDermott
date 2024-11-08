using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoridInWellnessProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "WellnessPrograms");

            migrationBuilder.AddColumn<long>(
                name: "AwarenessEduCategoryId",
                table: "WellnessPrograms",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellnessPrograms_AwarenessEduCategoryId",
                table: "WellnessPrograms",
                column: "AwarenessEduCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WellnessPrograms_AwarenessEduCategories_AwarenessEduCategoryId",
                table: "WellnessPrograms",
                column: "AwarenessEduCategoryId",
                principalTable: "AwarenessEduCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WellnessPrograms_AwarenessEduCategories_AwarenessEduCategoryId",
                table: "WellnessPrograms");

            migrationBuilder.DropIndex(
                name: "IX_WellnessPrograms_AwarenessEduCategoryId",
                table: "WellnessPrograms");

            migrationBuilder.DropColumn(
                name: "AwarenessEduCategoryId",
                table: "WellnessPrograms");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "WellnessPrograms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
