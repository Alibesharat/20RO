using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fkcascaced : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "AcademyId",
                table: "studentParents",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_studentParents_academies_AcademyId",
                table: "studentParents",
                column: "AcademyId",
                principalTable: "academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "AcademyId",
                table: "studentParents",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_studentParents_academies_AcademyId",
                table: "studentParents",
                column: "AcademyId",
                principalTable: "academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
