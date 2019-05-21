using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class generalseetig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralSetting",
                table: "GeneralSetting");

            migrationBuilder.RenameTable(
                name: "GeneralSetting",
                newName: "generalSettings");

            migrationBuilder.AddColumn<int>(
                name: "SeasionCount",
                table: "generalSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_generalSettings",
                table: "generalSettings",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_generalSettings",
                table: "generalSettings");

            migrationBuilder.DropColumn(
                name: "SeasionCount",
                table: "generalSettings");

            migrationBuilder.RenameTable(
                name: "generalSettings",
                newName: "GeneralSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralSetting",
                table: "GeneralSetting",
                column: "Id");
        }
    }
}
