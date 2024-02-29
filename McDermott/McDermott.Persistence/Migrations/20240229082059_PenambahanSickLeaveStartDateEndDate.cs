using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PenambahanSickLeaveStartDateEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateSickLeave",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSickLeave",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateSickLeave",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateSickLeave",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "IsSickLeave",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "StartDateSickLeave",
                table: "GeneralConsultanServices");
        }
    }
}
