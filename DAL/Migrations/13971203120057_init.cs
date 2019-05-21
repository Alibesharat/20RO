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
                name: "GeneralSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(nullable: true),
                    LogoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSetting", x => x.Id);
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
                    ClassName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentParents", x => x.Id);
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
                    HasPlan = table.Column<bool>(nullable: false)
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
                    AcademyCategoryId = table.Column<int>(nullable: false),
                    OtherPhoneNumber = table.Column<string>(nullable: true),
                    OfficeNumber = table.Column<string>(nullable: true)
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
                        name: "FK_academies_districts_districtId",
                        column: x => x.districtId,
                        principalTable: "districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    TeacherName = table.Column<string>(nullable: true),
                    AcademyId = table.Column<int>(nullable: false)
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
                name: "serviceRequsets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequsetCode = table.Column<string>(nullable: true),
                    StudentParrentId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    gender = table.Column<bool>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: false),
                    RequsetState = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.InsertData(
                table: "GeneralSetting",
                columns: new[] { "Id", "LogoPath", "SiteName" },
                values: new object[] { 1, "", "" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Hs_Change", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, false, "برنامه نویس" },
                    { 2, null, false, "مدیر" },
                    { 3, null, false, "کاربر  معمولی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_academies_AcademyCategoryId",
                table: "academies",
                column: "AcademyCategoryId");

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
                name: "IX_drivers_CityId",
                table: "drivers",
                column: "CityId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "GeneralSetting");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "serviceRequsets");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "studentParents");

            migrationBuilder.DropTable(
                name: "academies");

            migrationBuilder.DropTable(
                name: "academyCategories");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "provinces");
        }
    }
}
