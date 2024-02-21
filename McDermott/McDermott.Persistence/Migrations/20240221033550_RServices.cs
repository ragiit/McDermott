using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_Service_ServiceId",
                table: "DoctorSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Service_ServiceId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropForeignKey(
                name: "FK_KioskDepartements_Service_ServiceKId",
                table: "KioskDepartements");

            migrationBuilder.DropForeignKey(
                name: "FK_KioskDepartements_Service_ServicePId",
                table: "KioskDepartements");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosks_Service_ServiceId",
                table: "Kiosks");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Service_ServicedId1",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_ServicedId1",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ServicedId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ServicedId1",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_Services_ServiceId",
                table: "DoctorSchedules",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Services_ServiceId",
                table: "GeneralConsultanServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KioskDepartements_Services_ServiceKId",
                table: "KioskDepartements",
                column: "ServiceKId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KioskDepartements_Services_ServicePId",
                table: "KioskDepartements",
                column: "ServicePId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosks_Services_ServiceId",
                table: "Kiosks",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_Services_ServiceId",
                table: "DoctorSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServices_Services_ServiceId",
                table: "GeneralConsultanServices");

            migrationBuilder.DropForeignKey(
                name: "FK_KioskDepartements_Services_ServiceKId",
                table: "KioskDepartements");

            migrationBuilder.DropForeignKey(
                name: "FK_KioskDepartements_Services_ServicePId",
                table: "KioskDepartements");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosks_Services_ServiceId",
                table: "Kiosks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.AddColumn<string>(
                name: "ServicedId",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicedId1",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServicedId1",
                table: "Service",
                column: "ServicedId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_Service_ServiceId",
                table: "DoctorSchedules",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServices_Service_ServiceId",
                table: "GeneralConsultanServices",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KioskDepartements_Service_ServiceKId",
                table: "KioskDepartements",
                column: "ServiceKId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KioskDepartements_Service_ServicePId",
                table: "KioskDepartements",
                column: "ServicePId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosks_Service_ServiceId",
                table: "Kiosks",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Service_ServicedId1",
                table: "Service",
                column: "ServicedId1",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
