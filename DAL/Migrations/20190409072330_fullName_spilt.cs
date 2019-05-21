using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fullName_spilt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "students");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "students",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "students",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "students");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "students");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "students");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "students",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
