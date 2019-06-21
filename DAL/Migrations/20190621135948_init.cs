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
                name: "AcademyCategories",
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
                    table.PrimaryKey("PK_AcademyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CityPercent = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true, defaultValue: "Ntdf41vW0U6qYSFmcfqYRg"),
                    Name = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: true),
                    NIdCode = table.Column<string>(nullable: true),
                    RegisterCompanyNumber = table.Column<string>(nullable: true),
                    RelaseDate = table.Column<DateTime>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(nullable: true),
                    LogoPath = table.Column<string>(nullable: true),
                    TaxiPercent = table.Column<int>(nullable: false),
                    VanPercent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
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
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentParents",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AvatarPath = table.Column<string>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: true),
                    ClassName = table.Column<string>(nullable: true),
                    AccesptTerms = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentParents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    DistrictPercent = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true, defaultValue: "X2xu0B2pQUWSJdO0amnoCQ"),
                    Name = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: false),
                    CarName = table.Column<string>(nullable: true),
                    CarType = table.Column<string>(nullable: true),
                    CarColor = table.Column<string>(nullable: true),
                    IranianIdCode = table.Column<string>(nullable: true),
                    DrivingLicense = table.Column<string>(nullable: true),
                    DriverCode = table.Column<int>(nullable: false),
                    PelakNumber = table.Column<string>(nullable: true),
                    IsMaried = table.Column<bool>(nullable: false),
                    ContractorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Academies",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true, defaultValue: "LHnTW6JtB02TkDZcicPVgw"),
                    Name = table.Column<string>(nullable: true),
                    PhoneNubmber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AvatarPath = table.Column<string>(nullable: true, defaultValue: "Default.png"),
                    BeginDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: false),
                    Address = table.Column<string>(nullable: true),
                    DistrictId = table.Column<int>(nullable: false),
                    Latitude = table.Column<string>(nullable: true, defaultValue: "35.752308"),
                    Longtude = table.Column<string>(nullable: true, defaultValue: "51.399735"),
                    AcademyCategoryId = table.Column<int>(nullable: false),
                    SupportNumber = table.Column<string>(nullable: true),
                    ContractorId = table.Column<int>(nullable: false),
                    AcademyPercent = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Academies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Academies_AcademyCategories_AcademyCategoryId",
                        column: x => x.AcademyCategoryId,
                        principalTable: "AcademyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Academies_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Academies_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxiServices",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: false, defaultValue: "rDW1jcmyWUGtUSpAEkXgLg"),
                    Name = table.Column<string>(nullable: true),
                    DriverId = table.Column<int>(nullable: false),
                    TaxiCabState = table.Column<int>(nullable: false),
                    DriverPercent = table.Column<int>(nullable: false, defaultValue: 75),
                    ServiceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxiServices_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequsets",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: false, defaultValue: "JGIGZ0gdWkq6itZEuIVjGQ"),
                    StudentParrentId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    IrIdCod = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Latitue = table.Column<string>(nullable: true),
                    Longtude = table.Column<string>(nullable: true),
                    Distance = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    RequsetState = table.Column<int>(nullable: false),
                    NotifState = table.Column<int>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    AcademyId = table.Column<int>(nullable: false),
                    TaxiServiceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequsets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequsets_Academies_AcademyId",
                        column: x => x.AcademyId,
                        principalTable: "Academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequsets_StudentParents_StudentParrentId",
                        column: x => x.StudentParrentId,
                        principalTable: "StudentParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequsets_TaxiServices_TaxiServiceId",
                        column: x => x.TaxiServiceId,
                        principalTable: "TaxiServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accountings",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceRequsetId = table.Column<string>(nullable: true),
                    PayType = table.Column<int>(nullable: false),
                    Payed = table.Column<int>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: true),
                    NextPay = table.Column<DateTime>(nullable: true),
                    TrackNumber = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accountings_ServiceRequsets_ServiceRequsetId",
                        column: x => x.ServiceRequsetId,
                        principalTable: "ServiceRequsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GeneralSettings",
                columns: new[] { "Id", "LogoPath", "SiteName", "TaxiPercent", "VanPercent" },
                values: new object[] { 1, "", "", 2500, 2000 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Hs_Change", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, false, "برنامه نویس" },
                    { 2, null, false, "مدیر" },
                    { 3, null, false, "کاربر  معمولی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Academies_AcademyCategoryId",
                table: "Academies",
                column: "AcademyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Academies_ContractorId",
                table: "Academies",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Academies_DistrictId",
                table: "Academies",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountings_ServiceRequsetId",
                table: "Accountings",
                column: "ServiceRequsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ContractorId",
                table: "Drivers",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequsets_AcademyId",
                table: "ServiceRequsets",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequsets_StudentParrentId",
                table: "ServiceRequsets",
                column: "StudentParrentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequsets_TaxiServiceId",
                table: "ServiceRequsets",
                column: "TaxiServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxiServices_DriverId",
                table: "TaxiServices",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accountings");

            migrationBuilder.DropTable(
                name: "GeneralSettings");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ServiceRequsets");

            migrationBuilder.DropTable(
                name: "Academies");

            migrationBuilder.DropTable(
                name: "StudentParents");

            migrationBuilder.DropTable(
                name: "TaxiServices");

            migrationBuilder.DropTable(
                name: "AcademyCategories");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Contractors");
        }
    }
}
