using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class rebase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_Courses_CourseId",
                table: "serviceRequsets");

            migrationBuilder.DropForeignKey(
                name: "FK_studentParents_academies_AcademyId",
                table: "studentParents");

            migrationBuilder.DropForeignKey(
                name: "FK_studentParents_Courses_CourseId",
                table: "studentParents");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_studentParents_AcademyId",
                table: "studentParents");

            migrationBuilder.DropIndex(
                name: "IX_studentParents_CourseId",
                table: "studentParents");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_CourseId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "AcademyId",
                table: "studentParents");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "studentParents");

            migrationBuilder.DropColumn(
                name: "IrIdCod",
                table: "studentParents");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "studentParents");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "serviceRequsets");

            migrationBuilder.AddColumn<int>(
                name: "AcademyId",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IrIdCod",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_AcademyId",
                table: "serviceRequsets",
                column: "AcademyId");

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_academies_AcademyId",
                table: "serviceRequsets",
                column: "AcademyId",
                principalTable: "academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_academies_AcademyId",
                table: "serviceRequsets");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_AcademyId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "AcademyId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "IrIdCod",
                table: "serviceRequsets");

            migrationBuilder.AddColumn<int>(
                name: "AcademyId",
                table: "studentParents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "studentParents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IrIdCod",
                table: "studentParents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "studentParents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "serviceRequsets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "gender",
                table: "serviceRequsets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcademyId = table.Column<int>(nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TeacherName = table.Column<string>(nullable: true),
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
                name: "sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CoursId = table.Column<int>(nullable: false),
                    EndHour = table.Column<int>(nullable: false),
                    EndMiniute = table.Column<int>(nullable: false),
                    StartHour = table.Column<int>(nullable: false),
                    StartMiniute = table.Column<int>(nullable: false),
                    weekday = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_studentParents_AcademyId",
                table: "studentParents",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_studentParents_CourseId",
                table: "studentParents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_CourseId",
                table: "serviceRequsets",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AcademyId",
                table: "Courses",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_sections_CoursId",
                table: "sections",
                column: "CoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_Courses_CourseId",
                table: "serviceRequsets",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentParents_academies_AcademyId",
                table: "studentParents",
                column: "AcademyId",
                principalTable: "academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_studentParents_Courses_CourseId",
                table: "studentParents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
