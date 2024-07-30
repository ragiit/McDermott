﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeleteGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMenus_Groups_GroupId",
                table: "GroupMenus");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMenus_Groups_GroupId",
                table: "GroupMenus",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMenus_Groups_GroupId",
                table: "GroupMenus");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMenus_Groups_GroupId",
                table: "GroupMenus",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
