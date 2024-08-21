using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedVaccinatiponPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Maintainances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VaccinationPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    PhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    SalesPersonId = table.Column<long>(type: "bigint", nullable: true),
                    EducatorId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dose = table.Column<int>(type: "int", nullable: false),
                    NextDoseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_EducatorId",
                        column: x => x.EducatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_PhysicianId",
                        column: x => x.PhysicianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_EducatorId",
                table: "VaccinationPlans",
                column: "EducatorId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_GeneralConsultanServiceId",
                table: "VaccinationPlans",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_PatientId",
                table: "VaccinationPlans",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_PhysicianId",
                table: "VaccinationPlans",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_ProductId",
                table: "VaccinationPlans",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_SalesPersonId",
                table: "VaccinationPlans",
                column: "SalesPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccinationPlans");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Maintainances");
        }
    }
}
