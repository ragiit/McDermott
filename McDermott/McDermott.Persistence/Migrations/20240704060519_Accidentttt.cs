using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Accidentttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentStatus",
                table: "Accidents",
                newName: "Status");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "Accidents",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "Accidents",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "AreaOfYard",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeDescription",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GeneralConsultanServiceId",
                table: "Accidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Sent",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_GeneralConsultanServiceId",
                table: "Accidents",
                column: "GeneralConsultanServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "Accidents",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_GeneralConsultanServiceId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "EmployeeDescription",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "GeneralConsultanServiceId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Accidents");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Accidents",
                newName: "SentStatus");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "Accidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "Accidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AreaOfYard",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
