using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGensClinical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_Height",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "_Weight",
                table: "GeneralConsultantClinicalAssesments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "_Height",
                table: "GeneralConsultantClinicalAssesments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "_Weight",
                table: "GeneralConsultantClinicalAssesments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
