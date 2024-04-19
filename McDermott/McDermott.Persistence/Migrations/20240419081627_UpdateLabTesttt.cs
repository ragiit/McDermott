using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabTesttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_LabTests_LabTestId",
                table: "LabResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabUoms_LabUomId",
                table: "LabTests");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_SampleTypes_SampleTypeId",
                table: "LabTests");

            migrationBuilder.DropIndex(
                name: "IX_LabTests_LabUomId",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "LabUomId",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "NormalRangeFemale",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "NormalRangeMale",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "Parameter",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "LabTests");

            migrationBuilder.RenameColumn(
                name: "ResultValueType",
                table: "LabTests",
                newName: "Code");

            migrationBuilder.CreateTable(
                name: "LabTestDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestId = table.Column<long>(type: "bigint", nullable: true),
                    LabUomId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRangeMale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRangeFemale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValueType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTestDetails_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabTestDetails_LabUoms_LabUomId",
                        column: x => x.LabUomId,
                        principalTable: "LabUoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabTestDetails_LabTestId",
                table: "LabTestDetails",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestDetails_LabUomId",
                table: "LabTestDetails",
                column: "LabUomId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTestDetails_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabResulLabExaminationtId",
                principalTable: "LabTestDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_LabTestDetails_LabTestId",
                table: "LabResultDetails",
                column: "LabTestId",
                principalTable: "LabTestDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_SampleTypes_SampleTypeId",
                table: "LabTests",
                column: "SampleTypeId",
                principalTable: "SampleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTestDetails_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropForeignKey(
                name: "FK_LabResultDetails_LabTestDetails_LabTestId",
                table: "LabResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_SampleTypes_SampleTypeId",
                table: "LabTests");

            migrationBuilder.DropTable(
                name: "LabTestDetails");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "LabTests",
                newName: "ResultValueType");

            migrationBuilder.AddColumn<long>(
                name: "LabUomId",
                table: "LabTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalRangeFemale",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalRangeMale",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parameter",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_LabUomId",
                table: "LabTests",
                column: "LabUomId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanMedicalSupports_LabTests_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabResulLabExaminationtId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabResultDetails_LabTests_LabTestId",
                table: "LabResultDetails",
                column: "LabTestId",
                principalTable: "LabTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabUoms_LabUomId",
                table: "LabTests",
                column: "LabUomId",
                principalTable: "LabUoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_SampleTypes_SampleTypeId",
                table: "LabTests",
                column: "SampleTypeId",
                principalTable: "SampleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
