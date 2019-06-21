using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class addadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "1Rkye9jGn0GdItPuLqKLyQ",
                oldClrType: typeof(string),
                oldDefaultValue: "rDW1jcmyWUGtUSpAEkXgLg");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "KdaHAvmO5UmgCwKxrcg5Q",
                oldClrType: typeof(string),
                oldDefaultValue: "JGIGZ0gdWkq6itZEuIVjGQ");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "P5sbMmmoaUiEAMzWQcz9Kw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "X2xu0B2pQUWSJdO0amnoCQ");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "bPOHUpZeXEWh7pq8zI2yQ",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "Ntdf41vW0U6qYSFmcfqYRg");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "buf6eFpNX0OLJ9DplADVg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "LHnTW6JtB02TkDZcicPVgw");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: false, defaultValue: "cS16OYtOFket9EfX5jyng"),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AllowActivity = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "AllowActivity", "Hs_Change", "IsDeleted", "Name", "Password", "Username" },
                values: new object[] { "QieTF96XUaHoF3pt5RYCw", false, null, false, "Admin", "123456789", "09195410188" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaxiServices",
                nullable: false,
                defaultValue: "rDW1jcmyWUGtUSpAEkXgLg",
                oldClrType: typeof(string),
                oldDefaultValue: "1Rkye9jGn0GdItPuLqKLyQ");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServiceRequsets",
                nullable: false,
                defaultValue: "JGIGZ0gdWkq6itZEuIVjGQ",
                oldClrType: typeof(string),
                oldDefaultValue: "KdaHAvmO5UmgCwKxrcg5Q");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Drivers",
                nullable: true,
                defaultValue: "X2xu0B2pQUWSJdO0amnoCQ",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "P5sbMmmoaUiEAMzWQcz9Kw");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Contractors",
                nullable: true,
                defaultValue: "Ntdf41vW0U6qYSFmcfqYRg",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "bPOHUpZeXEWh7pq8zI2yQ");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Academies",
                nullable: true,
                defaultValue: "LHnTW6JtB02TkDZcicPVgw",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "buf6eFpNX0OLJ9DplADVg");
        }
    }
}
