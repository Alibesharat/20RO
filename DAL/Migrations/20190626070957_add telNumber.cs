using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class addtelNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: "hFlkD0nDAkqruHYR1C9Zng");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "vg74fycYLkezePrI8BgyBA",
                oldClrType: typeof(string),
                oldDefaultValue: "YRpeZNwtUuR74bSPUzkw");

            migrationBuilder.AddColumn<string>(
                name: "telNumber",
                table: "StudentParents",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "yFVvBQRqxEKsd82kZyGNQg",
                oldClrType: typeof(string),
                oldDefaultValue: "PBsDmgSkIElFe19Q86rA");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "AtvyvYetkEGnw3Kj5msg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "58ve5upSFk8lVeYFsqw");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "HTzHQhXfU0CR9Fagsl5Cw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "4EKosTrAzEKVC7sSHcbGgA");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Admins",
                nullable: false,
                defaultValue: "B1a2Y5QX8E2rh84OsVlLqQ",
                oldClrType: typeof(string),
                oldDefaultValue: "FpxbNQtcV0ZXO7nuEpOmA");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "mmIbKydAECCfLP5ZbrIiQ",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "jcF1EH4U6KuukKbwvYHg");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "AllowActivity", "Hs_Change", "IsDeleted", "Name", "Password", "Username" },
                values: new object[] { "8zZoWwWaXUWbiLE6lG0K5A", false, null, false, "Admin", "123456789", "09195410188" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: "8zZoWwWaXUWbiLE6lG0K5A");

            migrationBuilder.DropColumn(
                name: "telNumber",
                table: "StudentParents");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "YRpeZNwtUuR74bSPUzkw",
                oldClrType: typeof(string),
                oldDefaultValue: "vg74fycYLkezePrI8BgyBA");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "PBsDmgSkIElFe19Q86rA",
                oldClrType: typeof(string),
                oldDefaultValue: "yFVvBQRqxEKsd82kZyGNQg");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "58ve5upSFk8lVeYFsqw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "AtvyvYetkEGnw3Kj5msg");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "4EKosTrAzEKVC7sSHcbGgA",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "HTzHQhXfU0CR9Fagsl5Cw");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Admins",
                nullable: false,
                defaultValue: "FpxbNQtcV0ZXO7nuEpOmA",
                oldClrType: typeof(string),
                oldDefaultValue: "B1a2Y5QX8E2rh84OsVlLqQ");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "jcF1EH4U6KuukKbwvYHg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "mmIbKydAECCfLP5ZbrIiQ");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "AllowActivity", "Hs_Change", "IsDeleted", "Name", "Password", "Username" },
                values: new object[] { "hFlkD0nDAkqruHYR1C9Zng", false, null, false, "Admin", "123456789", "09195410188" });
        }
    }
}
