using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RevisiTipedataAnc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PregnancyStatusP",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PregnancyStatusG",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PregnancyStatusA",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistorySC",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PregnancyStatusH",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UK",
                table: "GeneralConsultanServiceAncs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DJJ",
                table: "GeneralConsultanServiceAncDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistorySC",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "PregnancyStatusH",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "UK",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.AlterColumn<string>(
                name: "PregnancyStatusP",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PregnancyStatusG",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PregnancyStatusA",
                table: "GeneralConsultanServiceAncs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DJJ",
                table: "GeneralConsultanServiceAncDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}