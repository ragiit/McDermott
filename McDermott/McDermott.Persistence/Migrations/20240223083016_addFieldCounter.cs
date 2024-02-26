using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFieldCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralConsultanCPPTs");

            migrationBuilder.DropTable(
                name: "GeneralConsultanMedicalSupports");

            migrationBuilder.AddColumn<int>(
                name: "PhysicianId",
                table: "Counters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Counters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Counters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_PhysicianId",
                table: "Counters",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_ServiceId",
                table: "Counters",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_Services_ServiceId",
                table: "Counters",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_Users_PhysicianId",
                table: "Counters",
                column: "PhysicianId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_Services_ServiceId",
                table: "Counters");

            migrationBuilder.DropForeignKey(
                name: "FK_Counters_Users_PhysicianId",
                table: "Counters");

            migrationBuilder.DropIndex(
                name: "IX_Counters_PhysicianId",
                table: "Counters");

            migrationBuilder.DropIndex(
                name: "IX_Counters_ServiceId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "PhysicianId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Counters");

            migrationBuilder.CreateTable(
                name: "GeneralConsultanCPPTs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanCPPTs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralConsultanMedicalSupports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<int>(type: "int", nullable: true),
                    AlcoholEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlcoholEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlcoholNegative = table.Column<bool>(type: "bit", nullable: true),
                    AlcoholPositive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrugEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugNegative = table.Column<bool>(type: "bit", nullable: true),
                    DrugPositive = table.Column<bool>(type: "bit", nullable: true),
                    LabEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanMedicalSupports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanCPPTs_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports",
                column: "GeneralConsultanServiceId");
        }
    }
}
