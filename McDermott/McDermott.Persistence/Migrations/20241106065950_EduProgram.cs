using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EduProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AwarenessEduCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwarenessEduCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    HTMLContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HTMLMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaterialContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attendance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_AwarenessEduCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "AwarenessEduCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParticipanEdus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    EducationProgramId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipanEdus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipanEdus_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParticipanEdus_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_EventCategoryId",
                table: "EducationPrograms",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanEdus_EducationProgramId",
                table: "ParticipanEdus",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanEdus_PatientId",
                table: "ParticipanEdus",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipanEdus");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropTable(
                name: "AwarenessEduCategories");
        }
    }
}
