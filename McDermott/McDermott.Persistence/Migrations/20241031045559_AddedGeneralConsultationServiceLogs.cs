using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedGeneralConsultationServiceLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryAdjustmentId",
                table: "InventoryAdjusmentLogs");

            migrationBuilder.DropColumn(
                name: "GeneralConsultationMedicalSupportId",
                table: "GeneralConsultanMedicalSupportLogs");

            migrationBuilder.CreateTable(
                name: "GeneralConsultationServiceLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultationServiceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationServiceLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationServiceLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationServiceLogs_GeneralConsultanServiceId",
                table: "GeneralConsultationServiceLogs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationServiceLogs_UserById",
                table: "GeneralConsultationServiceLogs",
                column: "UserById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralConsultationServiceLogs");

            migrationBuilder.AddColumn<long>(
                name: "InventoryAdjustmentId",
                table: "InventoryAdjusmentLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GeneralConsultationMedicalSupportId",
                table: "GeneralConsultanMedicalSupportLogs",
                type: "bigint",
                nullable: true);
        }
    }
}
