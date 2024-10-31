using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldGCID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GeneralCosultanServiceId",
                table: "GeneralConsultanServiceAncs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "GeneralConsultanServiceAncs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServiceAncs_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralConsultanServiceAncs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralConsultanServiceAncs_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropIndex(
                name: "IX_GeneralConsultanServiceAncs_GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "GeneralConsultanServiceId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "GeneralCosultanServiceId",
                table: "GeneralConsultanServiceAncs");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "GeneralConsultanServiceAncs");
        }
    }
}
