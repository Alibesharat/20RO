using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "academyCategories",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    IsMobielVerifed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    AllowActivity = table.Column<bool>(nullable: false),
                    IsCenterAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "generalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(nullable: true),
                    LogoPath = table.Column<string>(nullable: true),
                    SeasionCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generalSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cities_provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_districts_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    IsMobielVerifed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: false),
                    CarName = table.Column<string>(nullable: true),
                    CarType = table.Column<string>(nullable: true),
                    CarColor = table.Column<string>(nullable: true),
                    IranianIdCode = table.Column<string>(nullable: true),
                    DrivingLicense = table.Column<string>(nullable: true),
                    DriverCode = table.Column<int>(nullable: false),
                    PelakNumber = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    IsMaried = table.Column<bool>(nullable: false),
                    HasPlan = table.Column<bool>(nullable: false),
                    ContractorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drivers_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_drivers_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "academies",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    IsMobielVerifed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: false),
                    BossName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    districtId = table.Column<int>(nullable: false),
                    latitude = table.Column<string>(nullable: true, defaultValue: "35.752308"),
                    Longtude = table.Column<string>(nullable: true, defaultValue: "51.399735"),
                    AcademyCategoryId = table.Column<int>(nullable: false),
                    OtherPhoneNumber = table.Column<string>(nullable: true),
                    OfficeNumber = table.Column<string>(nullable: true),
                    ContractorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_academies_academyCategories_AcademyCategoryId",
                        column: x => x.AcademyCategoryId,
                        principalTable: "academyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_academies_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_academies_districts_districtId",
                        column: x => x.districtId,
                        principalTable: "districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taxiCabs",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsCompelete = table.Column<bool>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    TaxiCabState = table.Column<int>(nullable: false),
                    DriverPercent = table.Column<int>(nullable: false, defaultValue: 80)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxiCabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taxiCabs_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    TeacherName = table.Column<string>(nullable: true),
                    AcademyId = table.Column<int>(nullable: false),
                    trafficPercent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_academies_AcademyId",
                        column: x => x.AcademyId,
                        principalTable: "academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pricings",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    AcademyId = table.Column<int>(nullable: true),
                    FormKilometer = table.Column<decimal>(nullable: false),
                    ToKilometer = table.Column<decimal>(nullable: false),
                    ConstPrice = table.Column<int>(nullable: false),
                    PricePerKilometer = table.Column<int>(nullable: false),
                    Comission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pricings_academies_AcademyId",
                        column: x => x.AcademyId,
                        principalTable: "academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    weekday = table.Column<int>(nullable: false),
                    StartHour = table.Column<int>(nullable: false),
                    StartMiniute = table.Column<int>(nullable: false),
                    EndHour = table.Column<int>(nullable: false),
                    EndMiniute = table.Column<int>(nullable: false),
                    CoursId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sections_Courses_CoursId",
                        column: x => x.CoursId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentParents",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    IsMobielVerifed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: true),
                    ClassName = table.Column<string>(nullable: true),
                    StudentCode = table.Column<string>(nullable: true),
                    IrIdCod = table.Column<string>(nullable: true),
                    AccesptTerms = table.Column<bool>(nullable: false),
                    AcademyId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentParents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studentParents_academies_AcademyId",
                        column: x => x.AcademyId,
                        principalTable: "academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentParents_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "serviceRequsets",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequsetCode = table.Column<string>(nullable: true),
                    StudentParrentId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    IamgePath = table.Column<string>(nullable: true),
                    gender = table.Column<bool>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    latitue = table.Column<string>(nullable: true),
                    Longtude = table.Column<string>(nullable: true),
                    Distance = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: false),
                    price = table.Column<int>(nullable: false),
                    RequsetState = table.Column<int>(nullable: false),
                    NotifState = table.Column<int>(nullable: false),
                    cabAsFirstId = table.Column<int>(nullable: true),
                    cabAsSecondId = table.Column<int>(nullable: true),
                    cabAsThirdId = table.Column<int>(nullable: true),
                    cabAsFourthId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceRequsets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_studentParents_StudentParrentId",
                        column: x => x.StudentParrentId,
                        principalTable: "studentParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_taxiCabs_cabAsFirstId",
                        column: x => x.cabAsFirstId,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_taxiCabs_cabAsFourthId",
                        column: x => x.cabAsFourthId,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_taxiCabs_cabAsSecondId",
                        column: x => x.cabAsSecondId,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_serviceRequsets_taxiCabs_cabAsThirdId",
                        column: x => x.cabAsThirdId,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "driverFactors",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    From = table.Column<DateTime>(nullable: true),
                    To = table.Column<DateTime>(nullable: true),
                    SeassionCount = table.Column<int>(nullable: false),
                    taxiCabeid = table.Column<int>(nullable: true),
                    serviceRequsetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driverFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driverFactors_serviceRequsets_serviceRequsetId",
                        column: x => x.serviceRequsetId,
                        principalTable: "serviceRequsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_driverFactors_taxiCabs_taxiCabeid",
                        column: x => x.taxiCabeid,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ammount = table.Column<string>(nullable: true),
                    RequsetServiceId = table.Column<int>(nullable: true),
                    ParrentId = table.Column<int>(nullable: false),
                    TrackingCode = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_studentParents_ParrentId",
                        column: x => x.ParrentId,
                        principalTable: "studentParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_serviceRequsets_RequsetServiceId",
                        column: x => x.RequsetServiceId,
                        principalTable: "serviceRequsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Hs_Change", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, false, "برنامه نویس" },
                    { 2, null, false, "مدیر" },
                    { 3, null, false, "کاربر  معمولی" }
                });

            migrationBuilder.InsertData(
                table: "generalSettings",
                columns: new[] { "Id", "LogoPath", "SeasionCount", "SiteName" },
                values: new object[] { 1, "", 0, "" });

            migrationBuilder.CreateIndex(
                name: "IX_academies_AcademyCategoryId",
                table: "academies",
                column: "AcademyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_academies_ContractorId",
                table: "academies",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_academies_districtId",
                table: "academies",
                column: "districtId");

            migrationBuilder.CreateIndex(
                name: "IX_cities_ProvinceId",
                table: "cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AcademyId",
                table: "Courses",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_districts_CityId",
                table: "districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_driverFactors_serviceRequsetId",
                table: "driverFactors",
                column: "serviceRequsetId");

            migrationBuilder.CreateIndex(
                name: "IX_driverFactors_taxiCabeid",
                table: "driverFactors",
                column: "taxiCabeid");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_CityId",
                table: "drivers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_ContractorId",
                table: "drivers",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ParrentId",
                table: "payments",
                column: "ParrentId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_RequsetServiceId",
                table: "payments",
                column: "RequsetServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_pricings_AcademyId",
                table: "pricings",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_sections_CoursId",
                table: "sections",
                column: "CoursId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_CourseId",
                table: "serviceRequsets",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_StudentParrentId",
                table: "serviceRequsets",
                column: "StudentParrentId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsFirstId",
                table: "serviceRequsets",
                column: "cabAsFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsFourthId",
                table: "serviceRequsets",
                column: "cabAsFourthId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsSecondId",
                table: "serviceRequsets",
                column: "cabAsSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsThirdId",
                table: "serviceRequsets",
                column: "cabAsThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_studentParents_AcademyId",
                table: "studentParents",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_studentParents_CourseId",
                table: "studentParents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_taxiCabs_DriverId",
                table: "taxiCabs",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driverFactors");

            migrationBuilder.DropTable(
                name: "generalSettings");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "pricings");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "serviceRequsets");

            migrationBuilder.DropTable(
                name: "studentParents");

            migrationBuilder.DropTable(
                name: "taxiCabs");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "academies");

            migrationBuilder.DropTable(
                name: "academyCategories");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "provinces");
        }
    }
}
