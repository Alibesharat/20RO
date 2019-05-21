using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class trackcabl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hs_Change",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "serviceRequsets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Hs_Change",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Courses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hs_Change",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "Hs_Change",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Courses");
        }
    }
}
