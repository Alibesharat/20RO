using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class add_some_proprty_to_cotractor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Contractors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIdCode",
                table: "Contractors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisterCompanyNumber",
                table: "Contractors",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RelaseDate",
                table: "Contractors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Contractors");

            migrationBuilder.DropColumn(
                name: "NIdCode",
                table: "Contractors");

            migrationBuilder.DropColumn(
                name: "RegisterCompanyNumber",
                table: "Contractors");

            migrationBuilder.DropColumn(
                name: "RelaseDate",
                table: "Contractors");
        }
    }
}
