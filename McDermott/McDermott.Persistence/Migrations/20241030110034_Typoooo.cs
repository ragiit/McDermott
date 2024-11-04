using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Typoooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralCosultanServiceId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.AlterColumn<long>(
                name: "GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "GeneralCosultanServiceId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
