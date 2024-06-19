using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    DateOfOccurrence = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfFirstTreatment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AreaOfYard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RibbonSpecialCase = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentStatus = table.Column<int>(type: "int", nullable: false),
                    SelectedEmployeeCauseOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury13 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury14 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeCauseOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedNatureOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NatureOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedPartOfBody1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfBody1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedTreatment1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treatment1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accidents_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accidents_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_DepartmentId",
                table: "Accidents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_EmployeeId",
                table: "Accidents",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accidents");
        }
    }
}
