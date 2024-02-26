using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFieldCountersK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceKId",
                table: "Counters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Counters_ServiceKId",
                table: "Counters",
                column: "ServiceKId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_Services_ServiceKId",
                table: "Counters",
                column: "ServiceKId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_Services_ServiceKId",
                table: "Counters");

            migrationBuilder.DropIndex(
                name: "IX_Counters_ServiceKId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "ServiceKId",
                table: "Counters");
        }
    }
}
