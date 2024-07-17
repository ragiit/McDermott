using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Editabletransactionsese : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_ConcoctionLines_ConcoctionLineId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_Prescriptions_PrescriptionId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingId",
                table: "TransactionStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_ConcoctionLineId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_PrescriptionId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_ReceivingId",
                table: "TransactionStocks");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStocks_TransferId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "ConcoctionLineId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "InStock",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "OutStock",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "TransactionStocks");

            migrationBuilder.DropColumn(
                name: "ReceivingId",
                table: "TransactionStocks");

            migrationBuilder.RenameColumn(
                name: "TransferId",
                table: "TransactionStocks",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "TransactionStocks",
                newName: "TransferId");

            migrationBuilder.AddColumn<long>(
                name: "ConcoctionLineId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InStock",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OutStock",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrescriptionId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceivingId",
                table: "TransactionStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ConcoctionLineId",
                table: "TransactionStocks",
                column: "ConcoctionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_PrescriptionId",
                table: "TransactionStocks",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ReceivingId",
                table: "TransactionStocks",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_TransferId",
                table: "TransactionStocks",
                column: "TransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_ConcoctionLines_ConcoctionLineId",
                table: "TransactionStocks",
                column: "ConcoctionLineId",
                principalTable: "ConcoctionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_Prescriptions_PrescriptionId",
                table: "TransactionStocks",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_ReceivingStocks_ReceivingId",
                table: "TransactionStocks",
                column: "ReceivingId",
                principalTable: "ReceivingStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStocks_TransferStocks_TransferId",
                table: "TransactionStocks",
                column: "TransferId",
                principalTable: "TransferStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
