using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MartialStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredId = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdCardAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardCountryId = table.Column<long>(type: "bigint", nullable: true),
                    IdCardProvinceId = table.Column<long>(type: "bigint", nullable: true),
                    IdCardCityId = table.Column<long>(type: "bigint", nullable: true),
                    IdCardDistrictId = table.Column<long>(type: "bigint", nullable: true),
                    IdCardVillageId = table.Column<long>(type: "bigint", nullable: true),
                    IdCardRtRw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardZip = table.Column<long>(type: "bigint", nullable: true),
                    DomicileAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomicileAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomicileCountryId = table.Column<long>(type: "bigint", nullable: true),
                    DomicileProvinceId = table.Column<long>(type: "bigint", nullable: true),
                    DomicileCityId = table.Column<long>(type: "bigint", nullable: true),
                    DomicileDistrictId = table.Column<long>(type: "bigint", nullable: true),
                    DomicileVillageId = table.Column<long>(type: "bigint", nullable: true),
                    DomicileRtRw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomicileZip = table.Column<long>(type: "bigint", nullable: true),
                    BiologicalMother = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherNIK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReligionId = table.Column<long>(type: "bigint", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Npwp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoBpjsKs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoBpjsTk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SipNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SipFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SipExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StrNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSameDomicileAddress = table.Column<bool>(type: "bit", nullable: false),
                    SpecialityId = table.Column<long>(type: "bigint", nullable: true),
                    UserPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobPositionId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    EmergencyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoRm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DegreeId = table.Column<long>(type: "bigint", nullable: true),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultData = table.Column<bool>(type: "bit", nullable: false),
                    IsPatient = table.Column<bool>(type: "bit", nullable: false),
                    IsUser = table.Column<bool>(type: "bit", nullable: false),
                    IsDoctor = table.Column<bool>(type: "bit", nullable: false),
                    IsPhysicion = table.Column<bool>(type: "bit", nullable: false),
                    IsNurse = table.Column<bool>(type: "bit", nullable: false),
                    IsPharmacy = table.Column<bool>(type: "bit", nullable: false),
                    IsMcu = table.Column<bool>(type: "bit", nullable: false),
                    IsHr = table.Column<bool>(type: "bit", nullable: false),
                    PhysicanCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEmployeeRelation = table.Column<bool>(type: "bit", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Legacy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SAP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Oracle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorServiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientAllergyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsWeatherPatientAllergyIds = table.Column<bool>(type: "bit", nullable: false),
                    IsPharmacologyPatientAllergyIds = table.Column<bool>(type: "bit", nullable: false),
                    IsFoodPatientAllergyIds = table.Column<bool>(type: "bit", nullable: false),
                    WeatherPatientAllergyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacologyPatientAllergyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodPatientAllergyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    OccupationalId = table.Column<long>(type: "bigint", nullable: true),
                    IsFamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistoryOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMedicationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
