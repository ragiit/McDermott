using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTabeQueueDisplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QueueDisplayId",
                table: "Counters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QueueDisplays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueDisplays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Counters_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId");

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
                name: "QueueDisplays");

            migrationBuilder.DropIndex(
                name: "IX_Counters_QueueDisplayId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "QueueDisplayId",
                table: "Counters");
        }
    }
}
