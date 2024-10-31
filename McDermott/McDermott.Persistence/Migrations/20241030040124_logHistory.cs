using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class logHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                table: "GeneralConsultationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultationLogs_Users_UserById",
                table: "GeneralConsultationLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralConsultationLogs",
                table: "GeneralConsultationLogs");

            migrationBuilder.RenameTable(
                name: "GeneralConsultationLogs",
                newName: "GeneralConsultanServiceLogs");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultationLogs_UserById",
                table: "GeneralConsultanServiceLogs",
                newName: "IX_GeneralConsultanServiceLogs_UserById");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultationLogs_ProcedureRoomId",
                table: "GeneralConsultanServiceLogs",
                newName: "IX_GeneralConsultanServiceLogs_ProcedureRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultationLogs_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceLogs",
                newName: "IX_GeneralConsultanServiceLogs_GeneralConsultanServiceId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "GeneralConsultanServiceLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralConsultanServiceLogs",
                table: "GeneralConsultanServiceLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GeneralConsultanMedicalSupportLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GeneralConsultanMedicalSupportId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanMedicalSupportLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupportLogs_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                        column: x => x.GeneralConsultanMedicalSupportId,
                        principalTable: "GeneralConsultanMedicalSupports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupportLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjusmentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryAdjusmentId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjusmentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentLogs_InventoryAdjusments_InventoryAdjusmentId",
                        column: x => x.InventoryAdjusmentId,
                        principalTable: "InventoryAdjusments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupportLogs_GeneralConsultanMedicalSupportId",
                table: "GeneralConsultanMedicalSupportLogs",
                column: "GeneralConsultanMedicalSupportId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupportLogs_UserById",
                table: "GeneralConsultanMedicalSupportLogs",
                column: "UserById");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentLogs_InventoryAdjusmentId",
                table: "InventoryAdjusmentLogs",
                column: "InventoryAdjusmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentLogs_UserById",
                table: "InventoryAdjusmentLogs",
                column: "UserById");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServiceLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                table: "GeneralConsultanServiceLogs",
                column: "ProcedureRoomId",
                principalTable: "GeneralConsultanMedicalSupports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServiceLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceLogs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServiceLogs_Users_UserById",
                table: "GeneralConsultanServiceLogs",
                column: "UserById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServiceLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                table: "GeneralConsultanServiceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServiceLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServiceLogs_Users_UserById",
                table: "GeneralConsultanServiceLogs");

            migrationBuilder.DropTable(
                name: "GeneralConsultanMedicalSupportLogs");

            migrationBuilder.DropTable(
                name: "InventoryAdjusmentLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralConsultanServiceLogs",
                table: "GeneralConsultanServiceLogs");

            migrationBuilder.RenameTable(
                name: "GeneralConsultanServiceLogs",
                newName: "GeneralConsultationLogs");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultanServiceLogs_UserById",
                table: "GeneralConsultationLogs",
                newName: "IX_GeneralConsultationLogs_UserById");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultanServiceLogs_ProcedureRoomId",
                table: "GeneralConsultationLogs",
                newName: "IX_GeneralConsultationLogs_ProcedureRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralConsultanServiceLogs_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                newName: "IX_GeneralConsultationLogs_GeneralConsultanServiceId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "GeneralConsultationLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralConsultationLogs",
                table: "GeneralConsultationLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                table: "GeneralConsultationLogs",
                column: "ProcedureRoomId",
                principalTable: "GeneralConsultanMedicalSupports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultationLogs_Users_UserById",
                table: "GeneralConsultationLogs",
                column: "UserById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
