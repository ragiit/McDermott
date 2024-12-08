using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PenambahanFieldRujukan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReferDiagnosisNonSpesialis",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PracticeScheduleTimeDate",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferDiagnosisKd",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferDiagnosisNm",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReferSelectFaskesDate",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReferralExpiry",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralNo",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReferDiagnosisNonSpesialis",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "PracticeScheduleTimeDate",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferDiagnosisKd",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferDiagnosisNm",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferSelectFaskesDate",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferralExpiry",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferralNo",
                table: "GeneralConsultanServices");
        }
    }
}