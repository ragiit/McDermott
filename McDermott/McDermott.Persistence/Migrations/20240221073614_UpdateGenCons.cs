using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenCons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StagingStatus",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "GeneralConsultanServices",
                newName: "StagingStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StagingStatus",
                table: "GeneralConsultanServices",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "StagingStatus",
                table: "GeneralConsultantClinicalAssesments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
