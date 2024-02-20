using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableKioskconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StageBpjs",
                table: "Kiosks",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KioskConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KioskConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KioskConfigs_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KioskDepartements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceKId = table.Column<int>(type: "int", nullable: true),
                    ServicePId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KioskDepartements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KioskDepartements_Services_ServiceKId",
                        column: x => x.ServiceKId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KioskDepartements_Services_ServicePId",
                        column: x => x.ServicePId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KioskConfigs_ServiceId",
                table: "KioskConfigs",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskDepartements_ServiceKId",
                table: "KioskDepartements",
                column: "ServiceKId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskDepartements_ServicePId",
                table: "KioskDepartements",
                column: "ServicePId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KioskConfigs");

            migrationBuilder.DropTable(
                name: "KioskDepartements");

            migrationBuilder.DropColumn(
                name: "StageBpjs",
                table: "Kiosks");
        }
    }
}
