using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAncDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BB",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "DJJ",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "KU",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "Suhu",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "TD",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "TFU",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "TT",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "Trimester",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "UK",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.RenameColumn(
                name: "InspectionInitials",
                table: "GeneralConsultanServiceAncs",
                newName: "PregnancyStatusP");

            migrationBuilder.RenameColumn(
                name: "FetusPosition",
                table: "GeneralConsultanServiceAncs",
                newName: "PregnancyStatusG");

            migrationBuilder.RenameColumn(
                name: "Complaint",
                table: "GeneralConsultanServiceAncs",
                newName: "PregnancyStatusA");

            migrationBuilder.AddColumn<DateTime>(
                name: "PatientNextVisitSchedule",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceAnc",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PatientId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HPHT",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HPL",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LILA",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeneralConsultanServiceAncDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceAncId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Trimester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complaint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KU = table.Column<int>(type: "int", nullable: false),
                    TD = table.Column<int>(type: "int", nullable: false),
                    Suhu = table.Column<int>(type: "int", nullable: false),
                    BB = table.Column<int>(type: "int", nullable: false),
                    UK = table.Column<int>(type: "int", nullable: false),
                    TFU = table.Column<int>(type: "int", nullable: false),
                    FetusPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DJJ = table.Column<int>(type: "int", nullable: false),
                    TT = table.Column<int>(type: "int", nullable: false),
                    InspectionInitials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanServiceAncDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServiceAncDetails_GeneralConsultanServiceAncs_GeneralConsultanServiceAncId",
                        column: x => x.GeneralConsultanServiceAncId,
                        principalTable: "GeneralConsultanServiceAncs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServiceAncDetails_GeneralConsultanServiceAncId",
                table: "GeneralConsultanServiceAncDetails",
                column: "GeneralConsultanServiceAncId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralConsultanServiceAncDetails");

            migrationBuilder.DropColumn(
                name: "PatientNextVisitSchedule",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferenceAnc",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "HPHT",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "HPL",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "LILA",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.RenameColumn(
                name: "PregnancyStatusP",
                table: "GeneralConsultanServiceAncs",
                newName: "InspectionInitials");

            migrationBuilder.RenameColumn(
                name: "PregnancyStatusG",
                table: "GeneralConsultanServiceAncs",
                newName: "FetusPosition");

            migrationBuilder.RenameColumn(
                name: "PregnancyStatusA",
                table: "GeneralConsultanServiceAncs",
                newName: "Complaint");

            migrationBuilder.AlterColumn<long>(
                name: "PatientId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "BB",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DJJ",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "GeneralConsultanServiceAncs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "GeneralConsultanServiceAncs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "KU",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Suhu",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TD",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TFU",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TT",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Trimester",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UK",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
