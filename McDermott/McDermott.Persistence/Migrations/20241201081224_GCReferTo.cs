using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GCReferTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GCReferToInternals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    TypeClaim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRJMCINT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hospital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryRJMCINT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupationalId = table.Column<long>(type: "bigint", nullable: true),
                    TempDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TherapyProvide = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InpatientClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GCReferToInternals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GCReferToInternals_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GCReferToInternals_GeneralConsultanServiceId",
                table: "GCReferToInternals",
                column: "GeneralConsultanServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GCReferToInternals");
        }
    }
}