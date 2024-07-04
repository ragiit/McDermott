using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBPJSKesehatanAndTK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBPJS",
                table: "Insurances",
                newName: "IsBPJSTK");

            migrationBuilder.AddColumn<bool>(
                name: "IsBPJSKesehatan",
                table: "Insurances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBPJSKesehatan",
                table: "Insurances");

            migrationBuilder.RenameColumn(
                name: "IsBPJSTK",
                table: "Insurances",
                newName: "IsBPJS");
        }
    }
}
