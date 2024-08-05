using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Provinces_ProvinceId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CityId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProvinceId",
                table: "Districts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                table: "Districts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CityId1",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProvinceId1",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId1",
                table: "Districts",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId1",
                table: "Districts",
                column: "ProvinceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId1",
                table: "Districts",
                column: "CityId1",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Provinces_ProvinceId1",
                table: "Districts",
                column: "ProvinceId1",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId1",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Provinces_ProvinceId1",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CityId1",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_ProvinceId1",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "CityId1",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "ProvinceId1",
                table: "Districts");

            migrationBuilder.AlterColumn<long>(
                name: "ProvinceId",
                table: "Districts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<long>(
                name: "CityId",
                table: "Districts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Provinces_ProvinceId",
                table: "Districts",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
