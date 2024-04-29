using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class frequencyindrugdosage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Signas_SignaId",
                table: "Medicaments");

            migrationBuilder.AddColumn<long>(
                name: "FrequencyId",
                table: "Medicaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_FrequencyId",
                table: "Medicaments",
                column: "FrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_DrugDosages_FrequencyId",
                table: "Medicaments",
                column: "FrequencyId",
                principalTable: "DrugDosages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Signas_SignaId",
                table: "Medicaments",
                column: "SignaId",
                principalTable: "Signas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_DrugDosages_FrequencyId",
                table: "Medicaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Signas_SignaId",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_FrequencyId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "FrequencyId",
                table: "Medicaments");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Signas_SignaId",
                table: "Medicaments",
                column: "SignaId",
                principalTable: "Signas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
