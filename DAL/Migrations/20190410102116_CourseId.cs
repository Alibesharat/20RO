using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class CourseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "studentParents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_studentParents_CourseId",
                table: "studentParents",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentParents_Courses_CourseId",
                table: "studentParents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentParents_Courses_CourseId",
                table: "studentParents");

            migrationBuilder.DropIndex(
                name: "IX_studentParents_CourseId",
                table: "studentParents");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "studentParents");
        }
    }
}
