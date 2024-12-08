using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RujukanMCINT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryRJMCINT",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRJMCINT",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExamFor",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hospital",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InpatientClass",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JobPositionId",
                table: "GeneralConsultanServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferTo",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialist",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TempDiagnosis",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TherapyProvide",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeClaim",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_JobPositionId",
                table: "GeneralConsultanServices",
                column: "JobPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_JobPositions_JobPositionId",
                table: "GeneralConsultanServices",
                column: "JobPositionId",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_JobPositions_JobPositionId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServices_JobPositionId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "CategoryRJMCINT",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "DateRJMCINT",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ExamFor",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Hospital",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "InpatientClass",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "JobPositionId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferTo",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "Specialist",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "TempDiagnosis",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "TherapyProvide",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "TypeClaim",
                table: "GeneralConsultanServices");
        }
    }
}