using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class acepetterms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccesptTerms",
                table: "students",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccesptTerms",
                table: "students");
        }
    }
}
