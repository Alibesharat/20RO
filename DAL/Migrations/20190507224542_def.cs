using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class def : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DriverPercent",
                table: "taxiCabs",
                nullable: false,
                defaultValue: 80,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DriverPercent",
                table: "taxiCabs",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 80);
        }
    }
}
