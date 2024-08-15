using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Employees");
        }
    }
}
