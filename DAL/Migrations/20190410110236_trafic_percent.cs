using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class trafic_percent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "trafficPercent",
                table: "Courses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trafficPercent",
                table: "Courses");
        }
    }
}
