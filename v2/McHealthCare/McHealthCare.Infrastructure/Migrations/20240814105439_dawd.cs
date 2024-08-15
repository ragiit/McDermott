using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHealthCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dawd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingLocations_Locations_LocationId",
                table: "BuildingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Locations_LocationId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Cities_CityId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Countries_CountryId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Provinces_ProvinceId",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_ProvinceId",
                table: "Location",
                newName: "IX_Location_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_CountryId",
                table: "Location",
                newName: "IX_Location_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_CityId",
                table: "Location",
                newName: "IX_Location_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingLocations_Location_LocationId",
                table: "BuildingLocations",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Location_LocationId",
                table: "Buildings",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Cities_CityId",
                table: "Location",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Countries_CountryId",
                table: "Location",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Provinces_ProvinceId",
                table: "Location",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingLocations_Location_LocationId",
                table: "BuildingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Location_LocationId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Cities_CityId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Countries_CountryId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Provinces_ProvinceId",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Location_ProvinceId",
                table: "Locations",
                newName: "IX_Locations_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Location_CountryId",
                table: "Locations",
                newName: "IX_Locations_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Location_CityId",
                table: "Locations",
                newName: "IX_Locations_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingLocations_Locations_LocationId",
                table: "BuildingLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Locations_LocationId",
                table: "Buildings",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Cities_CityId",
                table: "Locations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Countries_CountryId",
                table: "Locations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Provinces_ProvinceId",
                table: "Locations",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
