using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPharmacyPresciptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    PractitionerId = table.Column<long>(type: "bigint", nullable: true),
                    PrescriptionLocationId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsWeather = table.Column<bool>(type: "bit", nullable: false),
                    IsFarmacologi = table.Column<bool>(type: "bit", nullable: false),
                    IsFood = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Users_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: false),
                    DrugFromId = table.Column<long>(type: "bigint", nullable: true),
                    DrugRouteId = table.Column<long>(type: "bigint", nullable: true),
                    DrugDosageId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosageFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugRoutName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<long>(type: "bigint", nullable: true),
                    Dosage = table.Column<long>(type: "bigint", nullable: true),
                    GivenAmount = table.Column<long>(type: "bigint", nullable: true),
                    PriceUnit = table.Column<long>(type: "bigint", nullable: true),
                    DrugFormId = table.Column<long>(type: "bigint", nullable: true),
                    SignaId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_DrugDosages_DrugDosageId",
                        column: x => x.DrugDosageId,
                        principalTable: "DrugDosages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_DrugRoutes_DrugRouteId",
                        column: x => x.DrugRouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_FormDrugs_DrugFormId",
                        column: x => x.DrugFormId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Signas_SignaId",
                        column: x => x.SignaId,
                        principalTable: "Signas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_LocationId",
                table: "Pharmacies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_MedicamentGroupId",
                table: "Pharmacies",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PatientId",
                table: "Pharmacies",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PractitionerId",
                table: "Pharmacies",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_ServiceId",
                table: "Pharmacies",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugDosageId",
                table: "Prescriptions",
                column: "DrugDosageId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugFormId",
                table: "Prescriptions",
                column: "DrugFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugRouteId",
                table: "Prescriptions",
                column: "DrugRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicamentGroupId",
                table: "Prescriptions",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PharmacyId",
                table: "Prescriptions",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ProductId",
                table: "Prescriptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_SignaId",
                table: "Prescriptions",
                column: "SignaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Pharmacies");
        }
    }
}
