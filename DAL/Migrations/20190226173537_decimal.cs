using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class @decimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Distance",
                table: "serviceRequsets",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ToKilometer",
                table: "pricings",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "FormKilometer",
                table: "pricings",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "serviceRequsets");

            migrationBuilder.AlterColumn<int>(
                name: "ToKilometer",
                table: "pricings",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "FormKilometer",
                table: "pricings",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
