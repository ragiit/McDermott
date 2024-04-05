using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditDetailMedical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitOfDosageCategory",
                table: "MedicamentGroupDetails",
                newName: "MedicaneDosage");

            migrationBuilder.RenameColumn(
                name: "UnitOfDosage",
                table: "MedicamentGroupDetails",
                newName: "Dosage");

            migrationBuilder.RenameColumn(
                name: "MedicaneId",
                table: "MedicamentGroupDetails",
                newName: "SignaId");

            migrationBuilder.AlterColumn<string>(
                name: "TotalQty",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QtyByDay",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MedicaneUnitDosage",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MedicamentGroupId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Days",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowSubtitation",
                table: "MedicamentGroupDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MedicamentId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegimentOfUseId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowSubtitation",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropColumn(
                name: "MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropColumn(
                name: "RegimentOfUseId",
                table: "MedicamentGroupDetails");

            migrationBuilder.RenameColumn(
                name: "SignaId",
                table: "MedicamentGroupDetails",
                newName: "MedicaneId");

            migrationBuilder.RenameColumn(
                name: "MedicaneDosage",
                table: "MedicamentGroupDetails",
                newName: "UnitOfDosageCategory");

            migrationBuilder.RenameColumn(
                name: "Dosage",
                table: "MedicamentGroupDetails",
                newName: "UnitOfDosage");

            migrationBuilder.AlterColumn<float>(
                name: "TotalQty",
                table: "MedicamentGroupDetails",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "QtyByDay",
                table: "MedicamentGroupDetails",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "MedicaneUnitDosage",
                table: "MedicamentGroupDetails",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MedicamentGroupId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<float>(
                name: "Days",
                table: "MedicamentGroupDetails",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
