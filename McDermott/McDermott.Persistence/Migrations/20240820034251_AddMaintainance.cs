using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintainance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maintainances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestById = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sequence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsibleById = table.Column<long>(type: "bigint", nullable: true),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isCorrective = table.Column<bool>(type: "bit", nullable: true),
                    isPreventive = table.Column<bool>(type: "bit", nullable: true),
                    isInternal = table.Column<bool>(type: "bit", nullable: true),
                    isExternal = table.Column<bool>(type: "bit", nullable: true),
                    VendorBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recurrent = table.Column<bool>(type: "bit", nullable: true),
                    RepeatNumber = table.Column<int>(type: "int", nullable: true),
                    RepeatWork = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintainances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintainances_Products_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintainances_Users_RequestById",
                        column: x => x.RequestById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintainances_Users_ResponsibleById",
                        column: x => x.ResponsibleById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_EquipmentId",
                table: "Maintainances",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_RequestById",
                table: "Maintainances",
                column: "RequestById");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_ResponsibleById",
                table: "Maintainances",
                column: "ResponsibleById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maintainances");
        }
    }
}
