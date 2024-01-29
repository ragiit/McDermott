using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProcedureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CronisCategories_Diagnoses_DiagnosisId",
                table: "CronisCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DiseaseCategories_Diagnoses_DiagnosisId",
                table: "DiseaseCategories");

            migrationBuilder.DropIndex(
                name: "IX_DiseaseCategories_DiagnosisId",
                table: "DiseaseCategories");

            migrationBuilder.DropIndex(
                name: "IX_CronisCategories_DiagnosisId",
                table: "CronisCategories");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "DiseaseCategories");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "CronisCategories");

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code_Test = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_CronisCategoryId",
                table: "Diagnoses",
                column: "CronisCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DiseaseCategoryId",
                table: "Diagnoses",
                column: "DiseaseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_CronisCategories_CronisCategoryId",
                table: "Diagnoses",
                column: "CronisCategoryId",
                principalTable: "CronisCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_DiseaseCategories_DiseaseCategoryId",
                table: "Diagnoses",
                column: "DiseaseCategoryId",
                principalTable: "DiseaseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_CronisCategories_CronisCategoryId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_DiseaseCategories_DiseaseCategoryId",
                table: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_CronisCategoryId",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_DiseaseCategoryId",
                table: "Diagnoses");

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisId",
                table: "DiseaseCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisId",
                table: "CronisCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseCategories_DiagnosisId",
                table: "DiseaseCategories",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_CronisCategories_DiagnosisId",
                table: "CronisCategories",
                column: "DiagnosisId");

            migrationBuilder.AddForeignKey(
                name: "FK_CronisCategories_Diagnoses_DiagnosisId",
                table: "CronisCategories",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiseaseCategories_Diagnoses_DiagnosisId",
                table: "DiseaseCategories",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
