using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddParentDiseaseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_ChronicCategory_ChronicCategoryId",
                table: "Diagnoses");

            migrationBuilder.DropTable(
                name: "ChronicCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "DiseaseCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CronisCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CronisCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseCategories_ParentId",
                table: "DiseaseCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_CronisCategories_ChronicCategoryId",
                table: "Diagnoses",
                column: "ChronicCategoryId",
                principalTable: "CronisCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiseaseCategories_DiseaseCategories_ParentId",
                table: "DiseaseCategories",
                column: "ParentId",
                principalTable: "DiseaseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_CronisCategories_ChronicCategoryId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_DiseaseCategories_DiseaseCategories_ParentId",
                table: "DiseaseCategories");

            migrationBuilder.DropIndex(
                name: "IX_DiseaseCategories_ParentId",
                table: "DiseaseCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "DiseaseCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CronisCategories",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CronisCategories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChronicCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChronicCategory", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_ChronicCategory_ChronicCategoryId",
                table: "Diagnoses",
                column: "ChronicCategoryId",
                principalTable: "ChronicCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
