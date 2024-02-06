using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralConsultan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralConsultanServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    InsuranceId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    PratitionerId = table.Column<int>(type: "int", nullable: true),
                    ClassType = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionQueue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoRM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSchendule = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Users_PratitionerId",
                        column: x => x.PratitionerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_InsuranceId",
                table: "GeneralConsultanServices",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_PatientId",
                table: "GeneralConsultanServices",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_PratitionerId",
                table: "GeneralConsultanServices",
                column: "PratitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ServiceId",
                table: "GeneralConsultanServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralConsultanServices");
        }
    }
}
