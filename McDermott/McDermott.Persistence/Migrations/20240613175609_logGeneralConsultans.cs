using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class logGeneralConsultans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralConsultationLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    ProcedureRoomId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                        column: x => x.ProcedureRoomId,
                        principalTable: "GeneralConsultanMedicalSupports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_ProcedureRoomId",
                table: "GeneralConsultationLogs",
                column: "ProcedureRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_UserById",
                table: "GeneralConsultationLogs",
                column: "UserById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralConsultationLogs");
        }
    }
}
