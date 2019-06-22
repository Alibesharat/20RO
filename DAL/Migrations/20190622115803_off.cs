using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class off : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: "QieTF96XUaHoF3pt5RYCw");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "YRpeZNwtUuR74bSPUzkw",
                oldClrType: typeof(string),
                oldDefaultValue: "1Rkye9jGn0GdItPuLqKLyQ");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "PBsDmgSkIElFe19Q86rA",
                oldClrType: typeof(string),
                oldDefaultValue: "KdaHAvmO5UmgCwKxrcg5Q");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "58ve5upSFk8lVeYFsqw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "P5sbMmmoaUiEAMzWQcz9Kw");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "4EKosTrAzEKVC7sSHcbGgA",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "bPOHUpZeXEWh7pq8zI2yQ");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Admins",
                nullable: false,
                defaultValue: "FpxbNQtcV0ZXO7nuEpOmA",
                oldClrType: typeof(string),
                oldDefaultValue: "cS16OYtOFket9EfX5jyng");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "jcF1EH4U6KuukKbwvYHg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "buf6eFpNX0OLJ9DplADVg");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Hs_Change", "IsDeleted", "Name", "Password", "Username" },
                values: new object[] { "hFlkD0nDAkqruHYR1C9Zng", null, false, "Admin", "123456789", "09195410188" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: "hFlkD0nDAkqruHYR1C9Zng");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "1Rkye9jGn0GdItPuLqKLyQ",
                oldClrType: typeof(string),
                oldDefaultValue: "YRpeZNwtUuR74bSPUzkw");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "KdaHAvmO5UmgCwKxrcg5Q",
                oldClrType: typeof(string),
                oldDefaultValue: "PBsDmgSkIElFe19Q86rA");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "P5sbMmmoaUiEAMzWQcz9Kw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "58ve5upSFk8lVeYFsqw");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "bPOHUpZeXEWh7pq8zI2yQ",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "4EKosTrAzEKVC7sSHcbGgA");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Admins",
                nullable: false,
                defaultValue: "cS16OYtOFket9EfX5jyng",
                oldClrType: typeof(string),
                oldDefaultValue: "FpxbNQtcV0ZXO7nuEpOmA");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "buf6eFpNX0OLJ9DplADVg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "jcF1EH4U6KuukKbwvYHg");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Hs_Change", "IsDeleted", "Name", "Password", "Username" },
                values: new object[] { "QieTF96XUaHoF3pt5RYCw", null, false, "Admin", "123456789", "09195410188" });
        }
    }
}
