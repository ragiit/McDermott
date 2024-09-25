using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McDermott.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KdAllergy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NmAllergy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Awarenesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KdSadar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NmSadar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awarenesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BpjsClassifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BpjsClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CronisCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronisCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailQueueDisplays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KioskQueueId = table.Column<long>(type: "bigint", nullable: true),
                    ServicekId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    NumberQueue = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailQueueDisplays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiseaseCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ParentDiseaseCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiseaseCategories_DiseaseCategories_ParentDiseaseCategoryId",
                        column: x => x.ParentDiseaseCategoryId,
                        principalTable: "DiseaseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugRoutes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: true),
                    Smpt_Debug = table.Column<bool>(type: "bit", nullable: true),
                    Smtp_Encryption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smtp_Host = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Smtp_Pass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smtp_Port = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smtp_User = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InverseRelationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_Families_InverseRelationId",
                        column: x => x.InverseRelationId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormDrugs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDrugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDefaultData = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insurances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBPJSKesehatan = table.Column<bool>(type: "bit", nullable: false),
                    IsBPJSTK = table.Column<bool>(type: "bit", nullable: false),
                    AdminFee = table.Column<long>(type: "bigint", nullable: true),
                    Presentase = table.Column<long>(type: "bigint", nullable: true),
                    AdminFeeMax = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KioskConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KioskConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabUoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabUoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefaultData = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NursingDiagnoses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Problem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursingDiagnoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Occupationals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occupationals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code_Test = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryValuation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueDisplays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueDisplays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SampleTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Quota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPatient = table.Column<bool>(type: "bit", nullable: false),
                    IsKiosk = table.Column<bool>(type: "bit", nullable: false),
                    IsMcu = table.Column<bool>(type: "bit", nullable: false),
                    IsVaccination = table.Column<bool>(type: "bit", nullable: false),
                    IsTelemedicine = table.Column<bool>(type: "bit", nullable: false),
                    ServicedId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Services_ServicedId",
                        column: x => x.ServicedId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Signas",
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
                    table.PrimaryKey("PK_Signas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemParameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PCareBaseURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntreanFKTPBaseURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdAplikasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCareCodeProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UomCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UomCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiseaseCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CronisCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnoses_CronisCategories_CronisCategoryId",
                        column: x => x.CronisCategoryId,
                        principalTable: "CronisCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diagnoses_DiseaseCategories_DiseaseCategoryId",
                        column: x => x.DiseaseCategoryId,
                        principalTable: "DiseaseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugDosages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugRouteId = table.Column<long>(type: "bigint", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQtyPerDay = table.Column<float>(type: "real", nullable: false),
                    Days = table.Column<float>(type: "real", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugDosages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugDosages_DrugRoutes_DrugRouteId",
                        column: x => x.DrugRouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GroupMenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDefaultData = table.Column<bool>(type: "bit", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    IsCreate = table.Column<bool>(type: "bit", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    IsUpdate = table.Column<bool>(type: "bit", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    IsImport = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMenus_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabTests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTests_SampleTypes_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalTable: "SampleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    PhysicionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSchedules_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Uoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UomCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiggerRatio = table.Column<float>(type: "real", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoundingPrecision = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uoms_UomCategories_UomCategoryId",
                        column: x => x.UomCategoryId,
                        principalTable: "UomCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabTestDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestId = table.Column<long>(type: "bigint", nullable: true),
                    LabUomId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRangeMale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRangeFemale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValueType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTestDetails_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabTestDetails_LabUoms_LabUomId",
                        column: x => x.LabUomId,
                        principalTable: "LabUoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    Quota = table.Column<long>(type: "bigint", nullable: false),
                    UpdateToBpjs = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorScheduleDetails_DoctorSchedules_DoctorScheduleId",
                        column: x => x.DoctorScheduleId,
                        principalTable: "DoctorSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VAT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Street1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthCenters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCenters_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthCenters_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthCenters_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentLocationId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BpjsClassificationId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseUomId = table.Column<long>(type: "bigint", nullable: true),
                    TraceAbility = table.Column<bool>(type: "bit", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospitalType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfPurchase = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextCalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipmentCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTopicalMedication = table.Column<bool>(type: "bit", nullable: false),
                    IsOralMedication = table.Column<bool>(type: "bit", nullable: false),
                    HighAlert = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_BpjsClassifications_BpjsClassificationId",
                        column: x => x.BpjsClassificationId,
                        principalTable: "BpjsClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Uoms_PurchaseUomId",
                        column: x => x.PurchaseUomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Villages_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Villages_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Villages_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCenterId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_HealthCenters_HealthCenterId",
                        column: x => x.HealthCenterId,
                        principalTable: "HealthCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjusments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjusments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusments_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodeReceiving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingStocks_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReorderingRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumQuantity = table.Column<float>(type: "real", nullable: false),
                    MaximumQuantity = table.Column<float>(type: "real", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReorderingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReorderingRules_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReorderingRules_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
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
                    FrequencyId = table.Column<long>(type: "bigint", nullable: true),
                    RouteId = table.Column<long>(type: "bigint", nullable: true),
                    FormId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PregnancyWarning = table.Column<bool>(type: "bit", nullable: true),
                    Pharmacologi = table.Column<bool>(type: "bit", nullable: true),
                    Weather = table.Column<bool>(type: "bit", nullable: true),
                    Food = table.Column<bool>(type: "bit", nullable: true),
                    Cronies = table.Column<bool>(type: "bit", nullable: true),
                    MontlyMax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignaId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicaments_DrugDosages_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "DrugDosages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Medicaments_DrugRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Medicaments_FormDrugs_FormId",
                        column: x => x.FormId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medicaments_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StockProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinanceId = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    StatusTransaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockProducts_Locations_DestinanceId",
                        column: x => x.DestinanceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockProducts_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockProducts_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcTableId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    Validate = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStocks_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingLocations_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingStockDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingStockId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingStockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingStockDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReceivingStockDetails_ReceivingStocks_ReceivingStockId",
                        column: x => x.ReceivingStockId,
                        principalTable: "ReceivingStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingStockDetails_StockProducts_StockId",
                        column: x => x.StockId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransferStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    SchenduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodeTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockRequest = table.Column<bool>(type: "bit", nullable: true),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStocks_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStocks_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStocks_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjusmentDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryAdjusmentId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeoriticalQty = table.Column<long>(type: "bigint", nullable: false),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealQty = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjusmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentDetails_InventoryAdjusments_InventoryAdjusmentId",
                        column: x => x.InventoryAdjusmentId,
                        principalTable: "InventoryAdjusments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentDetails_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjusmentDetails_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferStockLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferStockId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStockLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStockLogs_Locations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransferStockLogs_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransferStockLogs_TransferStocks_TransferStockId",
                        column: x => x.TransferStockId,
                        principalTable: "TransferStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransferStockProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransferStockId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    QtyStock = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStockProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferStockProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferStockProduct_TransferStocks_TransferStockId",
                        column: x => x.TransferStockId,
                        principalTable: "TransferStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: false),
                    SafetyPersonnelId = table.Column<long>(type: "bigint", nullable: false),
                    DateOfOccurrence = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfFirstTreatment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RibbonSpecialCase = table.Column<bool>(type: "bit", nullable: false),
                    Sent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaOfYard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EmployeeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccidentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury13 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedEmployeeCauseOfInjury14 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeCauseOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCauseOfInjury14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedNatureOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedNatureOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NatureOfInjury1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfInjury8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedPartOfBody1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedPartOfBody12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfBody1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfBody12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedTreatment1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTreatment7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treatment1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActiveComponentMedicamentGroupDetail",
                columns: table => new
                {
                    ActiveComponentId = table.Column<long>(type: "bigint", nullable: false),
                    MedicamentGroupDetailsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveComponentMedicamentGroupDetail", x => new { x.ActiveComponentId, x.MedicamentGroupDetailsId });
                });

            migrationBuilder.CreateTable(
                name: "ActiveComponents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountOfComponent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcoctionLineId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentId = table.Column<long>(type: "bigint", nullable: true),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveComponents_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActiveComponents_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BPJSIntegrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePolicyId = table.Column<long>(type: "bigint", nullable: true),
                    NoKartu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HubunganKeluarga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TglLahir = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglMulaiAktif = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TglAkhirBerlaku = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GolDarah = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoHP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstProl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PstPrb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    KetAktif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tunggakan = table.Column<int>(type: "int", nullable: false),
                    KdProviderPstKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderPstNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiKdProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KdProviderGigiNmProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsKelasKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaNama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JnsPesertaKode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiKdAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNmAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiNoAsuransi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsuransiCob = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPJSIntegrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConcoctionLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcoctionId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentDosage = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentUnitOfDosage = table.Column<long>(type: "bigint", nullable: true),
                    Dosage = table.Column<long>(type: "bigint", nullable: true),
                    TotalQty = table.Column<long>(type: "bigint", nullable: true),
                    AvaliableQty = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcoctionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcoctionLines_Uoms_UomId",
                        column: x => x.UomId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockOutLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinesId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    CutStock = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOutLines_ConcoctionLines_LinesId",
                        column: x => x.LinesId,
                        principalTable: "ConcoctionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockOutLines_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Concoctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    PractitionerId = table.Column<long>(type: "bigint", nullable: true),
                    DrugFormId = table.Column<long>(type: "bigint", nullable: true),
                    DrugRouteId = table.Column<long>(type: "bigint", nullable: true),
                    DrugDosageId = table.Column<long>(type: "bigint", nullable: true),
                    ConcoctionQty = table.Column<long>(type: "bigint", nullable: true),
                    MedicamenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concoctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concoctions_DrugDosages_DrugDosageId",
                        column: x => x.DrugDosageId,
                        principalTable: "DrugDosages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_DrugRoutes_DrugRouteId",
                        column: x => x.DrugRouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concoctions_FormDrugs_DrugFormId",
                        column: x => x.DrugFormId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Counters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceKId = table.Column<long>(type: "bigint", nullable: true),
                    PhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueueDisplayId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counters_QueueDisplays_QueueDisplayId",
                        column: x => x.QueueDisplayId,
                        principalTable: "QueueDisplays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Counters_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Counters_Services_ServiceKId",
                        column: x => x.ServiceKId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    ParentDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DepartmentCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPositions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorScheduleSlots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    PhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorScheduleSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorScheduleSlots_DoctorSchedules_DoctorScheduleId",
                        column: x => x.DoctorScheduleId,
                        principalTable: "DoctorSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    From = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailFromId = table.Column<long>(type: "bigint", nullable: true),
                    ById = table.Column<long>(type: "bigint", nullable: true),
                    To = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ToPartnerId = table.Column<long>(type: "bigint", nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReplayTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Schendule = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentContent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeEmail = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_EmailSettings_EmailFromId",
                        column: x => x.EmailFromId,
                        principalTable: "EmailSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    NIP = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Legacy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SAP = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Oracle = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorServiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientAllergyIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    OccupationalId = table.Column<long>(type: "bigint", nullable: true),
                    IsFamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistoryOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMedicationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailTemplateId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Cities_DomicileCityId",
                        column: x => x.DomicileCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Cities_IdCardCityId",
                        column: x => x.IdCardCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Countries_DomicileCountryId",
                        column: x => x.DomicileCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Countries_IdCardCountryId",
                        column: x => x.IdCardCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Degrees_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Districts_DomicileDistrictId",
                        column: x => x.DomicileDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Districts_IdCardDistrictId",
                        column: x => x.IdCardDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_EmailTemplates_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Occupationals_OccupationalId",
                        column: x => x.OccupationalId,
                        principalTable: "Occupationals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Provinces_DomicileProvinceId",
                        column: x => x.DomicileProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Provinces_IdCardProvinceId",
                        column: x => x.IdCardProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Villages_DomicileVillageId",
                        column: x => x.DomicileVillageId,
                        principalTable: "Villages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Villages_IdCardVillageId",
                        column: x => x.IdCardVillageId,
                        principalTable: "Villages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    InsuranceId = table.Column<long>(type: "bigint", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Prolanis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParticipantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoCard = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Class = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MedicalRecordNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServicePPKName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServicePPKCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NursingClass = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Diagnosa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Poly = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Doctor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardPrintDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TmtDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TatDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParticipantStatus = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServiceParticipant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentAge = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgeAtTimeOfService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DinSos = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PronalisPBR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoSKTM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsuranceNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsuranceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kiosks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BPJS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageBpjs = table.Column<bool>(type: "bit", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kiosks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kiosks_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kiosks_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kiosks_Users_PhysicianId",
                        column: x => x.PhysicianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maintainances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestById = table.Column<long>(type: "bigint", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sequence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsibleById = table.Column<long>(type: "bigint", nullable: true),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isCorrective = table.Column<bool>(type: "bit", nullable: true),
                    isPreventive = table.Column<bool>(type: "bit", nullable: true),
                    isInternal = table.Column<bool>(type: "bit", nullable: true),
                    isExternal = table.Column<bool>(type: "bit", nullable: true),
                    VendorBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recurrent = table.Column<bool>(type: "bit", nullable: true),
                    RepeatNumber = table.Column<int>(type: "int", nullable: true),
                    RepeatWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintainances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintainances_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintainances_Products_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintainances_Users_RequestById",
                        column: x => x.RequestById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintainances_Users_ResponsibleById",
                        column: x => x.ResponsibleById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConcoction = table.Column<bool>(type: "bit", nullable: true),
                    PhycisianId = table.Column<long>(type: "bigint", nullable: true),
                    UoMId = table.Column<long>(type: "bigint", nullable: true),
                    FormDrugId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_FormDrugs_FormDrugId",
                        column: x => x.FormDrugId,
                        principalTable: "FormDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_Uoms_UoMId",
                        column: x => x.UoMId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroups_Users_PhycisianId",
                        column: x => x.PhycisianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Farmacology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmacologiCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weather = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeatherCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Food = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAllergies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientFamilyRelations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    FamilyMemberId = table.Column<long>(type: "bigint", nullable: false),
                    FamilyId = table.Column<long>(type: "bigint", nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFamilyRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientFamilyRelations_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFamilyRelations_Users_FamilyMemberId",
                        column: x => x.FamilyMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFamilyRelations_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_Locations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_ReceivingStocks_ReceivingId",
                        column: x => x.ReceivingId,
                        principalTable: "ReceivingStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KioskQueues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KioskId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceKId = table.Column<long>(type: "bigint", nullable: true),
                    QueueNumber = table.Column<long>(type: "bigint", nullable: true),
                    QueueStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueueStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KioskQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KioskQueues_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KioskQueues_Kiosks_KioskId",
                        column: x => x.KioskId,
                        principalTable: "Kiosks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KioskQueues_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KioskQueues_Services_ServiceKId",
                        column: x => x.ServiceKId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintainanceRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintainanceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintainanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintainanceRecords_Maintainances_MaintainanceId",
                        column: x => x.MaintainanceId,
                        principalTable: "Maintainances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintainanceRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentGroupDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicamentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    MedicamentId = table.Column<long>(type: "bigint", nullable: true),
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignaId = table.Column<long>(type: "bigint", nullable: true),
                    FrequencyId = table.Column<long>(type: "bigint", nullable: true),
                    UnitOfDosageId = table.Column<long>(type: "bigint", nullable: true),
                    Dosage = table.Column<long>(type: "bigint", nullable: true),
                    QtyByDay = table.Column<long>(type: "bigint", nullable: true),
                    Days = table.Column<long>(type: "bigint", nullable: true),
                    TotalQty = table.Column<long>(type: "bigint", nullable: true),
                    AllowSubtitation = table.Column<bool>(type: "bit", nullable: true),
                    MedicaneUnitDosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicaneDosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicaneName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentGroupDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_DrugDosages_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "DrugDosages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_MedicamentGroups_MedicamentGroupId",
                        column: x => x.MedicamentGroupId,
                        principalTable: "MedicamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_Products_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_Signas_SignaId",
                        column: x => x.SignaId,
                        principalTable: "Signas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentGroupDetails_Uoms_UnitOfDosageId",
                        column: x => x.UnitOfDosageId,
                        principalTable: "Uoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    Status = table.Column<int>(type: "int", nullable: true),
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
                name: "GeneralConsultanServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KioskQueueId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    InsurancePolicyId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PratitionerId = table.Column<long>(type: "bigint", nullable: true),
                    ClassTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionQueue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeMedical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAlertInformationSpecialCase = table.Column<bool>(type: "bit", nullable: false),
                    IsSickLeave = table.Column<bool>(type: "bit", nullable: false),
                    IsMaternityLeave = table.Column<bool>(type: "bit", nullable: false),
                    StartDateSickLeave = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateSickLeave = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartMaternityLeave = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndMaternityLeave = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalKhususCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalKhususCategoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalSpesialisParentSpesialisName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalSpesialisParentSpesialisCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalSpesialisParentSubSpesialisName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalSpesialisParentSubSpesialisCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSarana = table.Column<bool>(type: "bit", nullable: true),
                    ReferVerticalSpesialisSaranaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferVerticalSpesialisSaranaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPKRujukanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPKRujukanCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageToBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Markers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferDateVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MedexType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMcu = table.Column<bool>(type: "bit", nullable: false),
                    IsBatam = table.Column<bool>(type: "bit", nullable: false),
                    IsOutsideBatam = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusMCU = table.Column<int>(type: "int", nullable: false),
                    McuExaminationDocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    McuExaminationBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccidentExaminationDocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccidentExaminationBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScrinningTriageScale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskOfFalling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskOfFallingDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    RR = table.Column<long>(type: "bigint", nullable: false),
                    Temp = table.Column<long>(type: "bigint", nullable: false),
                    HR = table.Column<long>(type: "bigint", nullable: false),
                    PainScale = table.Column<long>(type: "bigint", nullable: false),
                    Systolic = table.Column<long>(type: "bigint", nullable: false),
                    DiastolicBP = table.Column<long>(type: "bigint", nullable: false),
                    SpO2 = table.Column<long>(type: "bigint", nullable: false),
                    Diastole = table.Column<long>(type: "bigint", nullable: false),
                    WaistCircumference = table.Column<long>(type: "bigint", nullable: false),
                    BMIIndex = table.Column<double>(type: "float", nullable: false),
                    BMIIndexString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BMIState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicVisitTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformationFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwarenessId = table.Column<long>(type: "bigint", nullable: true),
                    E = table.Column<long>(type: "bigint", nullable: false),
                    V = table.Column<long>(type: "bigint", nullable: false),
                    M = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    LinkMeet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Awarenesses_AwarenessId",
                        column: x => x.AwarenessId,
                        principalTable: "Awarenesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_InsurancePolicies_InsurancePolicyId",
                        column: x => x.InsurancePolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_KioskQueues_KioskQueueId",
                        column: x => x.KioskQueueId,
                        principalTable: "KioskQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanServices_Users_PratitionerId",
                        column: x => x.PratitionerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyLogs_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmacyLogs_Users_UserById",
                        column: x => x.UserById,
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
                    ActiveComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosageFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "GeneralConsultanCPPTs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanCPPTs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralConsultanMedicalSupports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PractitionerLabEximinationId = table.Column<long>(type: "bigint", nullable: true),
                    LabEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabResulLabExaminationtId = table.Column<long>(type: "bigint", nullable: true),
                    LabResulLabExaminationtIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PractitionerRadiologyEximinationId = table.Column<long>(type: "bigint", nullable: true),
                    RadiologyEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PractitionerAlcoholEximinationId = table.Column<long>(type: "bigint", nullable: true),
                    AlcoholEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlcoholEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlcoholNegative = table.Column<bool>(type: "bit", nullable: true),
                    AlcoholPositive = table.Column<bool>(type: "bit", nullable: true),
                    PractitionerDrugEximinationId = table.Column<long>(type: "bigint", nullable: true),
                    DrugEximinationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugEximinationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugNegative = table.Column<bool>(type: "bit", nullable: true),
                    DrugPositive = table.Column<bool>(type: "bit", nullable: true),
                    AmphetaminesNegative = table.Column<bool>(type: "bit", nullable: true),
                    AmphetaminesPositive = table.Column<bool>(type: "bit", nullable: true),
                    BenzodiazepinesNegative = table.Column<bool>(type: "bit", nullable: true),
                    BenzodiazepinesPositive = table.Column<bool>(type: "bit", nullable: true),
                    CocaineMetabolitesNegative = table.Column<bool>(type: "bit", nullable: true),
                    CocaineMetabolitesPositive = table.Column<bool>(type: "bit", nullable: true),
                    OpiatesNegative = table.Column<bool>(type: "bit", nullable: true),
                    OpiatesPositive = table.Column<bool>(type: "bit", nullable: true),
                    MethamphetaminesNegative = table.Column<bool>(type: "bit", nullable: true),
                    MethamphetaminesPositive = table.Column<bool>(type: "bit", nullable: true),
                    THCCannabinoidMarijuanaNegative = table.Column<bool>(type: "bit", nullable: true),
                    THCCannabinoidMarijuanaPositive = table.Column<bool>(type: "bit", nullable: true),
                    OtherExaminationAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECGAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOtherExaminationECG = table.Column<bool>(type: "bit", nullable: false),
                    OtherExaminationTypeECG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherExaminationRemarkECG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PractitionerECGId = table.Column<long>(type: "bigint", nullable: true),
                    IsNormalRestingECG = table.Column<bool>(type: "bit", nullable: false),
                    IsSinusRhythm = table.Column<bool>(type: "bit", nullable: false),
                    IsSinusBradycardia = table.Column<bool>(type: "bit", nullable: false),
                    IsSinusTachycardia = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    IsFirstTimeEnteringConfinedSpace = table.Column<bool>(type: "bit", nullable: false),
                    EnteringConfinedSpaceCount = table.Column<long>(type: "bigint", nullable: false),
                    IsDefectiveSenseOfSmell = table.Column<bool>(type: "bit", nullable: false),
                    IsAsthmaOrLungAilment = table.Column<bool>(type: "bit", nullable: false),
                    IsBackPainOrLimitationOfMobility = table.Column<bool>(type: "bit", nullable: false),
                    IsClaustrophobia = table.Column<bool>(type: "bit", nullable: false),
                    IsDiabetesOrHypoglycemia = table.Column<bool>(type: "bit", nullable: false),
                    IsEyesightProblem = table.Column<bool>(type: "bit", nullable: false),
                    IsFaintingSpellOrSeizureOrEpilepsy = table.Column<bool>(type: "bit", nullable: false),
                    IsHearingDisorder = table.Column<bool>(type: "bit", nullable: false),
                    IsHeartDiseaseOrDisorder = table.Column<bool>(type: "bit", nullable: false),
                    IsHighBloodPressure = table.Column<bool>(type: "bit", nullable: false),
                    IsLowerLimbsDeformity = table.Column<bool>(type: "bit", nullable: false),
                    IsMeniereDiseaseOrVertigo = table.Column<bool>(type: "bit", nullable: false),
                    RemarksMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateMedialHistory = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignatureEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    SignatureEmployeeImagesMedicalHistory = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureEmployeeImagesMedicalHistoryBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wt = table.Column<long>(type: "bigint", nullable: true),
                    Bp = table.Column<long>(type: "bigint", nullable: true),
                    Height = table.Column<long>(type: "bigint", nullable: true),
                    Pulse = table.Column<long>(type: "bigint", nullable: true),
                    ChestCircumference = table.Column<long>(type: "bigint", nullable: true),
                    AbdomenCircumference = table.Column<long>(type: "bigint", nullable: true),
                    RespiratoryRate = table.Column<long>(type: "bigint", nullable: true),
                    Temperature = table.Column<long>(type: "bigint", nullable: true),
                    IsConfinedSpace = table.Column<bool>(type: "bit", nullable: false),
                    Eye = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarNoseThroat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cardiovascular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respiratory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abdomen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extremities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Musculoskeletal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neurologic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpirometryTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespiratoryFitTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEximinedbyDoctor = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignatureEximinedDoctor = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureEximinedDoctorBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommended = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExaminedPhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    HR = table.Column<long>(type: "bigint", nullable: false),
                    IsOtherECG = table.Column<bool>(type: "bit", nullable: false),
                    OtherDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    LabTestId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultanMedicalSupports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_LabTestDetails_LabResulLabExaminationtId",
                        column: x => x.LabResulLabExaminationtId,
                        principalTable: "LabTestDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerAlcoholEximinationId",
                        column: x => x.PractitionerAlcoholEximinationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerDrugEximinationId",
                        column: x => x.PractitionerDrugEximinationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerECGId",
                        column: x => x.PractitionerECGId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerLabEximinationId",
                        column: x => x.PractitionerLabEximinationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultanMedicalSupports_Users_PractitionerRadiologyEximinationId",
                        column: x => x.PractitionerRadiologyEximinationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralConsultantClinicalAssesments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    RR = table.Column<long>(type: "bigint", nullable: false),
                    Temp = table.Column<long>(type: "bigint", nullable: false),
                    HR = table.Column<long>(type: "bigint", nullable: false),
                    PainScale = table.Column<long>(type: "bigint", nullable: false),
                    Systolic = table.Column<long>(type: "bigint", nullable: false),
                    DiastolicBP = table.Column<long>(type: "bigint", nullable: false),
                    SpO2 = table.Column<long>(type: "bigint", nullable: false),
                    Sistole = table.Column<long>(type: "bigint", nullable: false),
                    Diastole = table.Column<long>(type: "bigint", nullable: false),
                    WaistCircumference = table.Column<long>(type: "bigint", nullable: false),
                    BMIIndex = table.Column<double>(type: "float", nullable: false),
                    BMIIndexString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BMIState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicVisitTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwarenessId = table.Column<long>(type: "bigint", nullable: true),
                    E = table.Column<long>(type: "bigint", nullable: false),
                    V = table.Column<long>(type: "bigint", nullable: false),
                    M = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultantClinicalAssesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultantClinicalAssesments_Awarenesses_AwarenessId",
                        column: x => x.AwarenessId,
                        principalTable: "Awarenesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SickLeaves",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultansId = table.Column<long>(type: "bigint", nullable: true),
                    TypeLeave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SickLeaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId",
                        column: x => x.GeneralConsultansId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaccinationPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    PhysicianId = table.Column<long>(type: "bigint", nullable: true),
                    SalesPersonId = table.Column<long>(type: "bigint", nullable: true),
                    EducatorId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dose = table.Column<int>(type: "int", nullable: false),
                    NextDoseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_EducatorId",
                        column: x => x.EducatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_PhysicianId",
                        column: x => x.PhysicianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaccinationPlans_Users_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockOutPrescriptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionStockId = table.Column<long>(type: "bigint", nullable: true),
                    CutStock = table.Column<long>(type: "bigint", nullable: true),
                    StockProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutPrescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOutPrescriptions_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StockOutPrescriptions_StockProducts_StockProductId",
                        column: x => x.StockProductId,
                        principalTable: "StockProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockOutPrescriptions_TransactionStocks_TransactionStockId",
                        column: x => x.TransactionStockId,
                        principalTable: "TransactionStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GeneralConsultationLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanServiceId = table.Column<long>(type: "bigint", nullable: true),
                    ProcedureRoomId = table.Column<long>(type: "bigint", nullable: true),
                    UserById = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConsultationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId",
                        column: x => x.ProcedureRoomId,
                        principalTable: "GeneralConsultanMedicalSupports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId",
                        column: x => x.GeneralConsultanServiceId,
                        principalTable: "GeneralConsultanServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralConsultationLogs_Users_UserById",
                        column: x => x.UserById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabResultDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralConsultanMedicalSupportId = table.Column<long>(type: "bigint", nullable: false),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabUomId = table.Column<long>(type: "bigint", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValueType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResultDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResultDetails_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId",
                        column: x => x.GeneralConsultanMedicalSupportId,
                        principalTable: "GeneralConsultanMedicalSupports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabResultDetails_LabUoms_LabUomId",
                        column: x => x.LabUomId,
                        principalTable: "LabUoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_DepartmentId",
                table: "Accidents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_EmployeeId",
                table: "Accidents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_GeneralConsultanServiceId",
                table: "Accidents",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_SafetyPersonnelId",
                table: "Accidents",
                column: "SafetyPersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetailsId",
                table: "ActiveComponentMedicamentGroupDetail",
                column: "MedicamentGroupDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_ConcoctionLineId",
                table: "ActiveComponents",
                column: "ConcoctionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_MedicamentId",
                table: "ActiveComponents",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_PrescriptionId",
                table: "ActiveComponents",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveComponents_UomId",
                table: "ActiveComponents",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_BPJSIntegrations_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLocations_BuildingId",
                table: "BuildingLocations",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLocations_LocationId",
                table: "BuildingLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HealthCenterId",
                table: "Buildings",
                column: "HealthCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CityId",
                table: "Companies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProvinceId",
                table: "Companies",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_ConcoctionId",
                table: "ConcoctionLines",
                column: "ConcoctionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_ProductId",
                table: "ConcoctionLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoctionLines_UomId",
                table: "ConcoctionLines",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_DrugDosageId",
                table: "Concoctions",
                column: "DrugDosageId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_DrugFormId",
                table: "Concoctions",
                column: "DrugFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_DrugRouteId",
                table: "Concoctions",
                column: "DrugRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_MedicamentGroupId",
                table: "Concoctions",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_PharmacyId",
                table: "Concoctions",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Concoctions_PractitionerId",
                table: "Concoctions",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_PhysicianId",
                table: "Counters",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_QueueDisplayId",
                table: "Counters",
                column: "QueueDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_ServiceId",
                table: "Counters",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_ServiceKId",
                table: "Counters",
                column: "ServiceKId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerId",
                table: "Departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_CronisCategoryId",
                table: "Diagnoses",
                column: "CronisCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DiseaseCategoryId",
                table: "Diagnoses",
                column: "DiseaseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseCategories_ParentDiseaseCategoryId",
                table: "DiseaseCategories",
                column: "ParentDiseaseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name",
                table: "Districts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleDetails_DoctorScheduleId",
                table: "DoctorScheduleDetails",
                column: "DoctorScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_ServiceId",
                table: "DoctorSchedules",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleSlots_DoctorScheduleId",
                table: "DoctorScheduleSlots",
                column: "DoctorScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleSlots_PhysicianId",
                table: "DoctorScheduleSlots",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugDosages_DrugRouteId",
                table: "DrugDosages",
                column: "DrugRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ById",
                table: "EmailTemplates",
                column: "ById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_EmailFromId",
                table: "EmailTemplates",
                column: "EmailFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_InverseRelationId",
                table: "Families",
                column: "InverseRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanCPPTs_GeneralConsultanServiceId",
                table: "GeneralConsultanCPPTs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_EmployeeId",
                table: "GeneralConsultanMedicalSupports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_GeneralConsultanServiceId",
                table: "GeneralConsultanMedicalSupports",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabResulLabExaminationtId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabResulLabExaminationtId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_LabTestId",
                table: "GeneralConsultanMedicalSupports",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerAlcoholEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerAlcoholEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerDrugEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerDrugEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerECGId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerECGId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerLabEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerLabEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanMedicalSupports_PractitionerRadiologyEximinationId",
                table: "GeneralConsultanMedicalSupports",
                column: "PractitionerRadiologyEximinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_AwarenessId",
                table: "GeneralConsultanServices",
                column: "AwarenessId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ClassTypeId",
                table: "GeneralConsultanServices",
                column: "ClassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_InsurancePolicyId",
                table: "GeneralConsultanServices",
                column: "InsurancePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_KioskQueueId",
                table: "GeneralConsultanServices",
                column: "KioskQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_LocationId",
                table: "GeneralConsultanServices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_PatientId",
                table: "GeneralConsultanServices",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_PratitionerId",
                table: "GeneralConsultanServices",
                column: "PratitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ProjectId",
                table: "GeneralConsultanServices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultanServices_ServiceId",
                table: "GeneralConsultanServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultantClinicalAssesments_AwarenessId",
                table: "GeneralConsultantClinicalAssesments",
                column: "AwarenessId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultantClinicalAssesments_GeneralConsultanServiceId",
                table: "GeneralConsultantClinicalAssesments",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_GeneralConsultanServiceId",
                table: "GeneralConsultationLogs",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_ProcedureRoomId",
                table: "GeneralConsultationLogs",
                column: "ProcedureRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralConsultationLogs_UserById",
                table: "GeneralConsultationLogs",
                column: "UserById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMenus_GroupId",
                table: "GroupMenus",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMenus_MenuId",
                table: "GroupMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCenters_CityId",
                table: "HealthCenters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCenters_CountryId",
                table: "HealthCenters",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCenters_ProvinceId",
                table: "HealthCenters",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_InsuranceId",
                table: "InsurancePolicies",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_NoCard",
                table: "InsurancePolicies",
                column: "NoCard",
                unique: true,
                filter: "[NoCard] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_UserId",
                table: "InsurancePolicies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_InventoryAdjusmentId",
                table: "InventoryAdjusmentDetails",
                column: "InventoryAdjusmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_ProductId",
                table: "InventoryAdjusmentDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_StockProductId",
                table: "InventoryAdjusmentDetails",
                column: "StockProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusmentDetails_TransactionStockId",
                table: "InventoryAdjusmentDetails",
                column: "TransactionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusments_CompanyId",
                table: "InventoryAdjusments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjusments_LocationId",
                table: "InventoryAdjusments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_DepartmentId",
                table: "JobPositions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_ClassTypeId",
                table: "KioskQueues",
                column: "ClassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_KioskId",
                table: "KioskQueues",
                column: "KioskId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_ServiceId",
                table: "KioskQueues",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_KioskQueues_ServiceKId",
                table: "KioskQueues",
                column: "ServiceKId");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosks_PatientId",
                table: "Kiosks",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosks_PhysicianId",
                table: "Kiosks",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosks_ServiceId",
                table: "Kiosks",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResultDetails_GeneralConsultanMedicalSupportId",
                table: "LabResultDetails",
                column: "GeneralConsultanMedicalSupportId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResultDetails_LabUomId",
                table: "LabResultDetails",
                column: "LabUomId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestDetails_LabTestId",
                table: "LabTestDetails",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestDetails_LabUomId",
                table: "LabTestDetails",
                column: "LabUomId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_SampleTypeId",
                table: "LabTests",
                column: "SampleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CompanyId",
                table: "Locations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainanceRecords_MaintainanceId",
                table: "MaintainanceRecords",
                column: "MaintainanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainanceRecords_ProductId",
                table: "MaintainanceRecords",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_EquipmentId",
                table: "Maintainances",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_LocationId",
                table: "Maintainances",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_RequestById",
                table: "Maintainances",
                column: "RequestById");

            migrationBuilder.CreateIndex(
                name: "IX_Maintainances_ResponsibleById",
                table: "Maintainances",
                column: "ResponsibleById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_FrequencyId",
                table: "MedicamentGroupDetails",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_MedicamentGroupId",
                table: "MedicamentGroupDetails",
                column: "MedicamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_MedicamentId",
                table: "MedicamentGroupDetails",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_SignaId",
                table: "MedicamentGroupDetails",
                column: "SignaId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroupDetails_UnitOfDosageId",
                table: "MedicamentGroupDetails",
                column: "UnitOfDosageId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_FormDrugId",
                table: "MedicamentGroups",
                column: "FormDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_PhycisianId",
                table: "MedicamentGroups",
                column: "PhycisianId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentGroups_UoMId",
                table: "MedicamentGroups",
                column: "UoMId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_FormId",
                table: "Medicaments",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_FrequencyId",
                table: "Medicaments",
                column: "FrequencyId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Name",
                table: "Menus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_UserId",
                table: "PatientAllergies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyRelations_FamilyId",
                table: "PatientFamilyRelations",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyRelations_FamilyMemberId",
                table: "PatientFamilyRelations",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyRelations_PatientId",
                table: "PatientFamilyRelations",
                column: "PatientId");

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
                name: "IX_PharmacyLogs_PharmacyId",
                table: "PharmacyLogs",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyLogs_UserById",
                table: "PharmacyLogs",
                column: "UserById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Products_BpjsClassificationId",
                table: "Products",
                column: "BpjsClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PurchaseUomId",
                table: "Products",
                column: "PurchaseUomId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UomId",
                table: "Products",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CountryId",
                table: "Provinces",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Name",
                table: "Provinces",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_ReceivingId",
                table: "ReceivingLogs",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_SourceId",
                table: "ReceivingLogs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLogs_UserById",
                table: "ReceivingLogs",
                column: "UserById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStockDetails_ProductId",
                table: "ReceivingStockDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStockDetails_ReceivingStockId",
                table: "ReceivingStockDetails",
                column: "ReceivingStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStockDetails_StockId",
                table: "ReceivingStockDetails",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingStocks_DestinationId",
                table: "ReceivingStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReorderingRules_CompanyId",
                table: "ReorderingRules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReorderingRules_LocationId",
                table: "ReorderingRules",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServicedId",
                table: "Services",
                column: "ServicedId");

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaves_GeneralConsultansId",
                table: "SickLeaves",
                column: "GeneralConsultansId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutLines_LinesId",
                table: "StockOutLines",
                column: "LinesId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutLines_TransactionStockId",
                table: "StockOutLines",
                column: "TransactionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutPrescriptions_PrescriptionId",
                table: "StockOutPrescriptions",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutPrescriptions_StockProductId",
                table: "StockOutPrescriptions",
                column: "StockProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutPrescriptions_TransactionStockId",
                table: "StockOutPrescriptions",
                column: "TransactionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_DestinanceId",
                table: "StockProducts",
                column: "DestinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_ProductId",
                table: "StockProducts",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_SourceId",
                table: "StockProducts",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_UomId",
                table: "StockProducts",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_LocationId",
                table: "TransactionStocks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_ProductId",
                table: "TransactionStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStocks_UomId",
                table: "TransactionStocks",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockLogs_DestinationId",
                table: "TransferStockLogs",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockLogs_SourceId",
                table: "TransferStockLogs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockLogs_TransferStockId",
                table: "TransferStockLogs",
                column: "TransferStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockProduct_ProductId",
                table: "TransferStockProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStockProduct_TransferStockId",
                table: "TransferStockProduct",
                column: "TransferStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_DestinationId",
                table: "TransferStocks",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_SourceId",
                table: "TransferStocks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferStocks_StockProductId",
                table: "TransferStocks",
                column: "StockProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Uoms_UomCategoryId",
                table: "Uoms",
                column: "UomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DegreeId",
                table: "Users",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomicileCityId",
                table: "Users",
                column: "DomicileCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomicileCountryId",
                table: "Users",
                column: "DomicileCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomicileDistrictId",
                table: "Users",
                column: "DomicileDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomicileProvinceId",
                table: "Users",
                column: "DomicileProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomicileVillageId",
                table: "Users",
                column: "DomicileVillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailTemplateId",
                table: "Users",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardCityId",
                table: "Users",
                column: "IdCardCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardCountryId",
                table: "Users",
                column: "IdCardCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardDistrictId",
                table: "Users",
                column: "IdCardDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardProvinceId",
                table: "Users",
                column: "IdCardProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardVillageId",
                table: "Users",
                column: "IdCardVillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobPositionId",
                table: "Users",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Legacy",
                table: "Users",
                column: "Legacy",
                unique: true,
                filter: "[Legacy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NIP",
                table: "Users",
                column: "NIP",
                unique: true,
                filter: "[NIP] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OccupationalId",
                table: "Users",
                column: "OccupationalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Oracle",
                table: "Users",
                column: "Oracle",
                unique: true,
                filter: "[Oracle] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReligionId",
                table: "Users",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SAP",
                table: "Users",
                column: "SAP",
                unique: true,
                filter: "[SAP] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpecialityId",
                table: "Users",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SupervisorId",
                table: "Users",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_EducatorId",
                table: "VaccinationPlans",
                column: "EducatorId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_GeneralConsultanServiceId",
                table: "VaccinationPlans",
                column: "GeneralConsultanServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_PatientId",
                table: "VaccinationPlans",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_PhysicianId",
                table: "VaccinationPlans",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_ProductId",
                table: "VaccinationPlans",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationPlans_SalesPersonId",
                table: "VaccinationPlans",
                column: "SalesPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_CityId",
                table: "Villages",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_DistrictId",
                table: "Villages",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_Id",
                table: "Villages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Villages_Name",
                table: "Villages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_ProvinceId",
                table: "Villages",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Departments_DepartmentId",
                table: "Accidents",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_GeneralConsultanServices_GeneralConsultanServiceId",
                table: "Accidents",
                column: "GeneralConsultanServiceId",
                principalTable: "GeneralConsultanServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Users_EmployeeId",
                table: "Accidents",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Users_SafetyPersonnelId",
                table: "Accidents",
                column: "SafetyPersonnelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponentMedicamentGroupDetail_ActiveComponents_ActiveComponentId",
                table: "ActiveComponentMedicamentGroupDetail",
                column: "ActiveComponentId",
                principalTable: "ActiveComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetails_MedicamentGroupDetailsId",
                table: "ActiveComponentMedicamentGroupDetail",
                column: "MedicamentGroupDetailsId",
                principalTable: "MedicamentGroupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_ConcoctionLines_ConcoctionLineId",
                table: "ActiveComponents",
                column: "ConcoctionLineId",
                principalTable: "ConcoctionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveComponents_Prescriptions_PrescriptionId",
                table: "ActiveComponents",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId",
                table: "BPJSIntegrations",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcoctionLines_Concoctions_ConcoctionId",
                table: "ConcoctionLines",
                column: "ConcoctionId",
                principalTable: "Concoctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_MedicamentGroups_MedicamentGroupId",
                table: "Concoctions",
                column: "MedicamentGroupId",
                principalTable: "MedicamentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_Pharmacies_PharmacyId",
                table: "Concoctions",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Concoctions_Users_PractitionerId",
                table: "Concoctions",
                column: "PractitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_Users_PhysicianId",
                table: "Counters",
                column: "PhysicianId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorScheduleSlots_Users_PhysicianId",
                table: "DoctorScheduleSlots",
                column: "PhysicianId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_Users_ById",
                table: "EmailTemplates",
                column: "ById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPositions_Departments_DepartmentId",
                table: "JobPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_Users_ById",
                table: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Accidents");

            migrationBuilder.DropTable(
                name: "ActiveComponentMedicamentGroupDetail");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "BPJSIntegrations");

            migrationBuilder.DropTable(
                name: "BuildingLocations");

            migrationBuilder.DropTable(
                name: "Counters");

            migrationBuilder.DropTable(
                name: "DetailQueueDisplays");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "DoctorScheduleDetails");

            migrationBuilder.DropTable(
                name: "DoctorScheduleSlots");

            migrationBuilder.DropTable(
                name: "GeneralConsultanCPPTs");

            migrationBuilder.DropTable(
                name: "GeneralConsultantClinicalAssesments");

            migrationBuilder.DropTable(
                name: "GeneralConsultationLogs");

            migrationBuilder.DropTable(
                name: "GroupMenus");

            migrationBuilder.DropTable(
                name: "InventoryAdjusmentDetails");

            migrationBuilder.DropTable(
                name: "KioskConfigs");

            migrationBuilder.DropTable(
                name: "LabResultDetails");

            migrationBuilder.DropTable(
                name: "MaintainanceRecords");

            migrationBuilder.DropTable(
                name: "NursingDiagnoses");

            migrationBuilder.DropTable(
                name: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientFamilyRelations");

            migrationBuilder.DropTable(
                name: "PharmacyLogs");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "ReceivingLogs");

            migrationBuilder.DropTable(
                name: "ReceivingStockDetails");

            migrationBuilder.DropTable(
                name: "ReorderingRules");

            migrationBuilder.DropTable(
                name: "SickLeaves");

            migrationBuilder.DropTable(
                name: "StockOutLines");

            migrationBuilder.DropTable(
                name: "StockOutPrescriptions");

            migrationBuilder.DropTable(
                name: "SystemParameters");

            migrationBuilder.DropTable(
                name: "TransferStockLogs");

            migrationBuilder.DropTable(
                name: "TransferStockProduct");

            migrationBuilder.DropTable(
                name: "VaccinationPlans");

            migrationBuilder.DropTable(
                name: "ActiveComponents");

            migrationBuilder.DropTable(
                name: "MedicamentGroupDetails");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "QueueDisplays");

            migrationBuilder.DropTable(
                name: "CronisCategories");

            migrationBuilder.DropTable(
                name: "DiseaseCategories");

            migrationBuilder.DropTable(
                name: "DoctorSchedules");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "InventoryAdjusments");

            migrationBuilder.DropTable(
                name: "GeneralConsultanMedicalSupports");

            migrationBuilder.DropTable(
                name: "Maintainances");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "ReceivingStocks");

            migrationBuilder.DropTable(
                name: "TransactionStocks");

            migrationBuilder.DropTable(
                name: "TransferStocks");

            migrationBuilder.DropTable(
                name: "ConcoctionLines");

            migrationBuilder.DropTable(
                name: "Medicaments");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "HealthCenters");

            migrationBuilder.DropTable(
                name: "GeneralConsultanServices");

            migrationBuilder.DropTable(
                name: "LabTestDetails");

            migrationBuilder.DropTable(
                name: "StockProducts");

            migrationBuilder.DropTable(
                name: "Concoctions");

            migrationBuilder.DropTable(
                name: "Signas");

            migrationBuilder.DropTable(
                name: "Awarenesses");

            migrationBuilder.DropTable(
                name: "InsurancePolicies");

            migrationBuilder.DropTable(
                name: "KioskQueues");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "LabTests");

            migrationBuilder.DropTable(
                name: "LabUoms");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DrugDosages");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Insurances");

            migrationBuilder.DropTable(
                name: "ClassTypes");

            migrationBuilder.DropTable(
                name: "Kiosks");

            migrationBuilder.DropTable(
                name: "SampleTypes");

            migrationBuilder.DropTable(
                name: "BpjsClassifications");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "DrugRoutes");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MedicamentGroups");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "FormDrugs");

            migrationBuilder.DropTable(
                name: "Uoms");

            migrationBuilder.DropTable(
                name: "UomCategories");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropTable(
                name: "Occupationals");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "EmailSettings");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
