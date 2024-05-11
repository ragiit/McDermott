using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeInsurancePolicyBpjs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                table: "BPJSIntegrations");

            migrationBuilder.AddForeignKey(
                name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                table: "BPJSIntegrations");

            migrationBuilder.AddForeignKey(
                name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
