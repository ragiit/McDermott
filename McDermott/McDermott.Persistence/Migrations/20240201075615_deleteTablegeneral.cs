using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteTablegeneral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generalConsultanServices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generalConsultanServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    BirthDay = table.Column<DateOnly>(type: "date", nullable: true),
                    ClassType = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateSchendule = table.Column<DateOnly>(type: "date", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoRM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PratititonerId = table.Column<int>(type: "int", nullable: true),
                    TimeSchendule = table.Column<TimeOnly>(type: "time", nullable: true),
                    TypeRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generalConsultanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_generalConsultanServices_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_generalConsultanServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_generalConsultanServices_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_generalConsultanServices_InsuranceId",
                table: "generalConsultanServices",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_generalConsultanServices_PatientId",
                table: "generalConsultanServices",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_generalConsultanServices_ServiceId",
                table: "generalConsultanServices",
                column: "ServiceId");
        }
    }
}
