using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ftr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractorId",
                table: "drivers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractorId",
                table: "academies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_drivers_ContractorId",
                table: "drivers",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_academies_ContractorId",
                table: "academies",
                column: "ContractorId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_academies_Contractors_ContractorId",
                table: "academies");

            migrationBuilder.DropForeignKey(
                name: "FK_drivers_Contractors_ContractorId",
                table: "drivers");

            migrationBuilder.DropIndex(
                name: "IX_drivers_ContractorId",
                table: "drivers");

            migrationBuilder.DropIndex(
                name: "IX_academies_ContractorId",
                table: "academies");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "drivers");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "academies");
        }
    }
}
