using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableSigna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters");

            migrationBuilder.CreateTable(
                name: "Signas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signas", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters");

            migrationBuilder.DropTable(
                name: "Signas");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_QueueDisplays_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId",
                principalTable: "QueueDisplays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
