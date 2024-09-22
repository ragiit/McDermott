using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiseaseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCategory",
                table: "DiseaseCategories");

            migrationBuilder.AddColumn<long>(
                name: "ParentDiseaseCategoryId",
                table: "DiseaseCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseCategories_ParentDiseaseCategoryId",
                table: "DiseaseCategories",
                column: "ParentDiseaseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiseaseCategories_DiseaseCategories_ParentDiseaseCategoryId",
                table: "DiseaseCategories",
                column: "ParentDiseaseCategoryId",
                principalTable: "DiseaseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiseaseCategories_DiseaseCategories_ParentDiseaseCategoryId",
                table: "DiseaseCategories");

            migrationBuilder.DropIndex(
                name: "IX_DiseaseCategories_ParentDiseaseCategoryId",
                table: "DiseaseCategories");

            migrationBuilder.DropColumn(
                name: "ParentDiseaseCategoryId",
                table: "DiseaseCategories");

            migrationBuilder.AddColumn<string>(
                name: "ParentCategory",
                table: "DiseaseCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
