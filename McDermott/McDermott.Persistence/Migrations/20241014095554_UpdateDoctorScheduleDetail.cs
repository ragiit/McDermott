using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDoctorScheduleDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Quota",
                table: "DoctorScheduleSlots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ServiceId",
                table: "DoctorScheduleSlots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhysicionId",
                table: "DoctorSchedules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ServiceId",
                table: "DoctorScheduleDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_PhysicionId",
                table: "DoctorSchedules",
                column: "PhysicionId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleDetails_ServiceId",
                table: "DoctorScheduleDetails",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorScheduleDetails_Services_ServiceId",
                table: "DoctorScheduleDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_Users_PhysicionId",
                table: "DoctorSchedules",
                column: "PhysicionId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorScheduleDetails_Services_ServiceId",
                table: "DoctorScheduleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_Users_PhysicionId",
                table: "DoctorSchedules");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedules_PhysicionId",
                table: "DoctorSchedules");

            migrationBuilder.DropIndex(
                name: "IX_DoctorScheduleDetails_ServiceId",
                table: "DoctorScheduleDetails");

            migrationBuilder.DropColumn(
                name: "Quota",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropColumn(
                name: "PhysicionId",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "DoctorScheduleDetails");
        }
    }
}
