using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fk_pricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pricings_cities_CityId",
                table: "pricings");

            migrationBuilder.DropIndex(
                name: "IX_pricings_CityId",
                table: "pricings");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "pricings");

            migrationBuilder.AddColumn<int>(
                name: "AcademyId",
                table: "pricings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pricings_AcademyId",
                table: "pricings",
                column: "AcademyId");

            migrationBuilder.AddForeignKey(
                name: "FK_pricings_academies_AcademyId",
                table: "pricings",
                column: "AcademyId",
                principalTable: "academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pricings_academies_AcademyId",
                table: "pricings");

            migrationBuilder.DropIndex(
                name: "IX_pricings_AcademyId",
                table: "pricings");

            migrationBuilder.DropColumn(
                name: "AcademyId",
                table: "pricings");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "pricings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_pricings_CityId",
                table: "pricings",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_pricings_cities_CityId",
                table: "pricings",
                column: "CityId",
                principalTable: "cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
