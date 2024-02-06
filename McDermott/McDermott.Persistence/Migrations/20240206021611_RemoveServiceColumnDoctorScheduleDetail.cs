using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveServiceColumnDoctorScheduleDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorScheduleDetails_Services_ServiceId",
                table: "DoctorScheduleDetails");

            migrationBuilder.DropIndex(
                name: "IX_DoctorScheduleDetails_ServiceId",
                table: "DoctorScheduleDetails");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "DoctorScheduleDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "DoctorScheduleDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
