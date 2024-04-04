using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugMedicament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormDrugs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDrugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConcoction = table.Column<bool>(type: "bit", nullable: true),
                    PhycisianId = table.Column<long>(type: "bigint", nullable: true),
                    UoMId = table.Column<long>(type: "bigint", nullable: true),
                    FormDrugId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_FormDrugs_FormDrugId",
                        column: x => x.FormDrugId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_Uoms_UoMId",
                        column: x => x.UoMId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_Users_PhycisianId",
                        column: x => x.PhycisianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentGroupDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    MedicaneId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UoMId = table.Column<long>(type: "bigint", nullable: true),
                    MedicaneUnitDosage = table.Column<float>(type: "real", nullable: true),
                    QtyByDay = table.Column<float>(type: "real", nullable: true),
                    Days = table.Column<float>(type: "real", nullable: true),
                    TotalQty = table.Column<float>(type: "real", nullable: true),
                    MedicaneName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitOfDosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitOfDosageCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentGroupDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_Uoms_UoMId",
                        column: x => x.UoMId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActiveComponentMedicamentGroupDetail",
                columns: table => new
                {
                    ActiveComponentId = table.Column<long>(type: "bigint", nullable: false),
                    MedicamentGroupDetailsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveComponentMedicamentGroupDetail", x => new { x.ActiveComponentId, x.MedicamentGroupDetailsId });
                    table.ForeignKey(
                        name: "FK_ActiveComponentMedicamentGroupDetail_ActiveComponents_ActiveComponentId",
                        column: x => x.ActiveComponentId,
                        principalTable: "ActiveComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetails_MedicamentGroupDetailsId",
                        column: x => x.MedicamentGroupDetailsId,
                        principalTable: "MedicamentGroupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetailsId",
                table: "ActiveComponentMedicamentGroupDetail",
                column: "MedicamentGroupDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_MedicamentGroupId",
                table: "MedicamentGroupDetails",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_UoMId",
                table: "MedicamentGroupDetails",
                column: "UoMId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_FormDrugId",
                table: "MedicamentGroups",
                column: "FormDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_PhycisianId",
                table: "MedicamentGroups",
                column: "PhycisianId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_UoMId",
                table: "MedicamentGroups",
                column: "UoMId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveComponentMedicamentGroupDetail");

            migrationBuilder.DropTable(
                name: "MedicamentGroupDetails");

            migrationBuilder.DropTable(
                name: "MedicamentGroups");

            migrationBuilder.DropTable(
                name: "FormDrugs");
        }
    }
}
