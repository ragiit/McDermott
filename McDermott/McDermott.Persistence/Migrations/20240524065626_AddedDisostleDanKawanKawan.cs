using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDisostleDanKawanKawan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Diastole",
                table: "GeneralConsultantClinicalAssesments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Sistole",
                table: "GeneralConsultantClinicalAssesments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WaistCircumference",
                table: "GeneralConsultantClinicalAssesments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diastole",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "Sistole",
                table: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropColumn(
                name: "WaistCircumference",
                table: "GeneralConsultantClinicalAssesments");
        }
    }
}
