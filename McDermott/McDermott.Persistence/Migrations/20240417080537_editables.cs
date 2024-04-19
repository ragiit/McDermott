using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InputType",
                table: "GeneralInformations",
                newName: "HospitalType");

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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetails_MedicamentGroupDetailsId",
                        column: x => x.MedicamentGroupDetailsId,
                        principalTable: "MedicamentGroupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetailsId",
                table: "ActiveComponentMedicamentGroupDetail",
                column: "MedicamentGroupDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveComponentMedicamentGroupDetail");

            migrationBuilder.RenameColumn(
                name: "HospitalType",
                table: "GeneralInformations",
                newName: "InputType");
        }
    }
}
