using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "MedicamentGroupId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MedicamentId",
                table: "ActiveComponents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralInformations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    BpjsClasificationId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralInformations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralInformations_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralInformations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralInformations_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Medicaments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SignaId = table.Column<long>(type: "bigint", nullable: true),
                    RouteId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PregnancyWarning = table.Column<bool>(type: "bit", nullable: true),
                    Cronies = table.Column<bool>(type: "bit", nullable: true),
                    MontlyMax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicaments_DrugRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Medicaments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Medicaments_Signas_SignaId",
                        column: x => x.SignaId,
                        principalTable: "Signas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Medicaments_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_MedicamentId",
                table: "ActiveComponents",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_CompanyId",
                table: "GeneralInformations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_ProductCategoryId",
                table: "GeneralInformations",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_ProductId",
                table: "GeneralInformations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_UomId",
                table: "GeneralInformations",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_ProductId",
                table: "Medicaments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_RouteId",
                table: "Medicaments",
                column: "RouteId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_SignaId",
                table: "Medicaments",
                column: "SignaId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_UomId",
                table: "Medicaments",
                column: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_Medicaments_MedicamentId",
                table: "ActiveComponents",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveComponents_Medicaments_MedicamentId",
                table: "ActiveComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentGroupDetails_Medicaments_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropTable(
                name: "GeneralInformations");

            migrationBuilder.DropTable(
                name: "Medicaments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentGroupDetails_MedicamentId",
                table: "MedicamentGroupDetails");

            migrationBuilder.DropIndex(
                name: "IX_ActiveComponents_MedicamentId",
                table: "ActiveComponents");

            migrationBuilder.DropColumn(
                name: "MedicamentId",
                table: "ActiveComponents");

            migrationBuilder.AlterColumn<long>(
                name: "MedicamentGroupId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "MedicaneId",
                table: "MedicamentGroupDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfDosage",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfDosageCategory",
                table: "MedicamentGroupDetails",
                type: "nvarchar(max)",
                nullable: true);

           

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

           
        }
    }
}
