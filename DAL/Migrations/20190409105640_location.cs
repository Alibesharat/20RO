using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "latitude",
                table: "academies",
                nullable: true,
                defaultValue: "35.752308",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Longtude",
                table: "academies",
                nullable: true,
                defaultValue: "51.399735",
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "latitude",
                table: "academies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "35.752308");

            migrationBuilder.AlterColumn<string>(
                name: "Longtude",
                table: "academies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "51.399735");
        }
    }
}
