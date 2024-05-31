using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldFaskes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSarana",
                table: "GeneralConsultanServices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PPKRujukanCode",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PPKRujukanName",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReferDateVisit",
                table: "GeneralConsultanServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferReason",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalKhususCategoryCode",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalKhususCategoryName",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisParentSpesialisCode",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisParentSpesialisName",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisParentSubSpesialisCode",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisParentSubSpesialisName",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisSaranaCode",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferVerticalSpesialisSaranaName",
                table: "GeneralConsultanServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSarana",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "PPKRujukanCode",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "PPKRujukanName",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferDateVisit",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferReason",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalKhususCategoryCode",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalKhususCategoryName",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisParentSpesialisCode",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisParentSpesialisName",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisParentSubSpesialisCode",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisParentSubSpesialisName",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisSaranaCode",
                table: "GeneralConsultanServices");

            migrationBuilder.DropColumn(
                name: "ReferVerticalSpesialisSaranaName",
                table: "GeneralConsultanServices");
        }
    }
}
