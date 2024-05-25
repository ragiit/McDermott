using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConcoctionsLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Concoctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    PractitionerId = table.Column<long>(type: "bigint", nullable: true),
                    DrugFormId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    QtyByDay = table.Column<long>(type: "bigint", nullable: true),
                    Day = table.Column<long>(type: "bigint", nullable: true),
                    TotalQty = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concoctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concoctions_FormDrugs_DrugFormId",
                        column: x => x.DrugFormId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_Users_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConcoctionLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcoctionId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentDosage = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentUnitOfDosage = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    TotalQty = table.Column<long>(type: "bigint", nullable: true),
                    AvaliableQty = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcoctionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_ActiveComponents_ActiveComponentId",
                        column: x => x.ActiveComponentId,
                        principalTable: "ActiveComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_Concoctions_ConcoctionId",
                        column: x => x.ConcoctionId,
                        principalTable: "Concoctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_ActiveComponentId",
                table: "ConcoctionLines",
                column: "ActiveComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_ConcoctionId",
                table: "ConcoctionLines",
                column: "ConcoctionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_MedicamentGroupId",
                table: "ConcoctionLines",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_UomId",
                table: "ConcoctionLines",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_DrugFormId",
                table: "Concoctions",
                column: "DrugFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_MedicamentGroupId",
                table: "Concoctions",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_PharmacyId",
                table: "Concoctions",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_PractitionerId",
                table: "Concoctions",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_UomId",
                table: "Concoctions",
                column: "UomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcoctionLines");

            migrationBuilder.DropTable(
                name: "Concoctions");
        }
    }
}
