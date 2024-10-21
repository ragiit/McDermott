using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFlaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccident",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGC",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTelemedicine",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVaccination",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleSlots_ServiceId",
                table: "DoctorScheduleSlots",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorScheduleSlots_Services_ServiceId",
                table: "DoctorScheduleSlots",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorScheduleSlots_Services_ServiceId",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropIndex(
                name: "IX_DoctorScheduleSlots_ServiceId",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropColumn(
                name: "IsAccident",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsGC",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsTelemedicine",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsVaccination",
                table: "GeneralConsultanServices");
        }
    }
}
