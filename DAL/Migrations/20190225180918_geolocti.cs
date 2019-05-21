using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class geolocti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Longtude",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "latitue",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "serviceRequsets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Longtude",
                table: "academies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "latitude",
                table: "academies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longtude",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "latitue",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "price",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "Longtude",
                table: "academies");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "academies");
        }
    }
}
