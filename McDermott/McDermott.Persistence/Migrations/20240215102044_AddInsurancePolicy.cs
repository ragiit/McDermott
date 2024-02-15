using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInsurancePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {  
            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InsuranceId = table.Column<int>(type: "int", nullable: false),
                    PolicyNumber = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Prolanis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParticipantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoCard = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Class = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MedicalRecordNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServicePPKName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServicePPKCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NursingClass = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Diagnosa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Poly = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Doctor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardPrintDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TmtDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TatDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParticipantStatus = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServiceParticipant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentAge = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgeAtTimeOfService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DinSos = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PronalisPBR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoSKTM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsuranceNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsuranceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                }); 

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_InsuranceId",
                table: "InsurancePolicies",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_UserId",
                table: "InsurancePolicies",
                column: "UserId"); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { 

            migrationBuilder.DropTable(
                name: "InsurancePolicies"); 
        }
    }
}
