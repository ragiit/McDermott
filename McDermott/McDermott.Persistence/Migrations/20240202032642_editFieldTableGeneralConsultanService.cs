using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editFieldTableGeneralConsultanService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSchendule",
                table: "GeneralConsultanServices",
                newName: "WorkTo");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkFrom",
                table: "GeneralConsultanServices",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkFrom",
                table: "GeneralConsultanServices");

            migrationBuilder.RenameColumn(
                name: "WorkTo",
                table: "GeneralConsultanServices",
                newName: "TimeSchendule");
        }
    }
}
