using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class isScenterAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCenterAdmin",
                table: "Contractors",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCenterAdmin",
                table: "Contractors");
        }
    }
}
