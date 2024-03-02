using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class servicedRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameIndex(
                name: "IX_Services_ServicedId",
                table: "Service",
                newName: "IX_Service_ServicedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

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
                name: "FK_Service_Service_ServicedId",
                table: "Service",
                column: "ServicedId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_Service_Service_ServicedId",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ServicedId",
                table: "Services",
                newName: "IX_Services_ServicedId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Services_ServicedId",
                table: "Services",
                column: "ServicedId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}