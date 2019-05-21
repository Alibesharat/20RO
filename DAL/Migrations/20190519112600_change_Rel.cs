using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class change_Rel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_academies_Contractors_ContractorId",
                table: "academies");

            migrationBuilder.DropForeignKey(
                name: "FK_drivers_Contractors_ContractorId",
                table: "drivers");

            migrationBuilder.AlterColumn<int>(
                name: "ContractorId",
                table: "drivers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractorId",
                table: "academies",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_academies_Contractors_ContractorId",
                table: "academies",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_drivers_Contractors_ContractorId",
                table: "drivers",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_academies_Contractors_ContractorId",
                table: "academies");

            migrationBuilder.DropForeignKey(
                name: "FK_drivers_Contractors_ContractorId",
                table: "drivers");

            migrationBuilder.AlterColumn<int>(
                name: "ContractorId",
                table: "drivers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ContractorId",
                table: "academies",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_academies_Contractors_ContractorId",
                table: "academies",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_drivers_Contractors_ContractorId",
                table: "drivers",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
