using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class افزدون_وضعیت_تاکسی_سرویس : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxiCabState",
                table: "taxiCabs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxiCabState",
                table: "taxiCabs");
        }
    }
}
