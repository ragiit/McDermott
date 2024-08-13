using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editIeld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "code",
                table: "Services",
                newName: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Services",
                newName: "code");
        }
    }
}
