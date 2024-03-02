using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystolicDiastolicBP",
                table: "GeneralConsultantClinicalAssesments",
                newName: "Systolic");

            migrationBuilder.AddColumn<int>(
                name: "DiastolicBP",
                table: "GeneralConsultantClinicalAssesments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiastolicBP",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.RenameColumn(
                name: "Systolic",
                table: "GeneralConsultantClinicalAssesments",
                newName: "SystolicDiastolicBP");
        }
    }
}