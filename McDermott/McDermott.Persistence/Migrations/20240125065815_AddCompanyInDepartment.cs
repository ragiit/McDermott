using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyInDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCategory",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "Departments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentCategory",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "Departments");
        }
    }
}