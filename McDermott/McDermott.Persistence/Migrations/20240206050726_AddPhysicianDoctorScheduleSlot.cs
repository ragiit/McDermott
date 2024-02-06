using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPhysicianDoctorScheduleSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhysicianId",
                table: "DoctorScheduleSlots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleSlots_PhysicianId",
                table: "DoctorScheduleSlots",
                column: "PhysicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorScheduleSlots_Users_PhysicianId",
                table: "DoctorScheduleSlots",
                column: "PhysicianId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorScheduleSlots_Users_PhysicianId",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropIndex(
                name: "IX_DoctorScheduleSlots_PhysicianId",
                table: "DoctorScheduleSlots");

            migrationBuilder.DropColumn(
                name: "PhysicianId",
                table: "DoctorScheduleSlots");
        }
    }
}
