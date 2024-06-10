using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DxBlazorApplication2.Module.Migrations
{
    /// <inheritdoc />
    public partial class D : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PositionID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AddressID",
                table: "Employees",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionID",
                table: "Employees",
                column: "PositionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Addresses_AddressID",
                table: "Employees",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionID",
                table: "Employees",
                column: "PositionID",
                principalTable: "Positions",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Addresses_AddressID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AddressID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PositionID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Employees");
        }
    }
}
