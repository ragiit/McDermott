using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFamilyMembersz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientFamilyRelations_families_FamilyId",
                table: "PatientFamilyRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_families",
                table: "families");

            migrationBuilder.RenameTable(
                name: "families",
                newName: "Families");

            migrationBuilder.AddColumn<string>(
                name: "ChildRelation",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentRelation",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Families",
                table: "Families",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientFamilyRelations_Families_FamilyId",
                table: "PatientFamilyRelations",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientFamilyRelations_Families_FamilyId",
                table: "PatientFamilyRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Families",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "ChildRelation",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "ParentRelation",
                table: "Families");

            migrationBuilder.RenameTable(
                name: "Families",
                newName: "families");

            migrationBuilder.AddPrimaryKey(
                name: "PK_families",
                table: "families",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientFamilyRelations_families_FamilyId",
                table: "PatientFamilyRelations",
                column: "FamilyId",
                principalTable: "families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
