using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class taxiCab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cabAsFirstId",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cabAsFourthId",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cabAsSecondId",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cabAsThirdId",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "taxiCabs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DriverId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxiCabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taxiCabs_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsFirstId",
                table: "serviceRequsets",
                column: "cabAsFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsFourthId",
                table: "serviceRequsets",
                column: "cabAsFourthId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsSecondId",
                table: "serviceRequsets",
                column: "cabAsSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceRequsets_cabAsThirdId",
                table: "serviceRequsets",
                column: "cabAsThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_taxiCabs_DriverId",
                table: "taxiCabs",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsFirstId",
                table: "serviceRequsets",
                column: "cabAsFirstId",
                principalTable: "taxiCabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsFourthId",
                table: "serviceRequsets",
                column: "cabAsFourthId",
                principalTable: "taxiCabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsSecondId",
                table: "serviceRequsets",
                column: "cabAsSecondId",
                principalTable: "taxiCabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsThirdId",
                table: "serviceRequsets",
                column: "cabAsThirdId",
                principalTable: "taxiCabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsFirstId",
                table: "serviceRequsets");

            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsFourthId",
                table: "serviceRequsets");

            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsSecondId",
                table: "serviceRequsets");

            migrationBuilder.DropForeignKey(
                name: "FK_serviceRequsets_taxiCabs_cabAsThirdId",
                table: "serviceRequsets");

            migrationBuilder.DropTable(
                name: "taxiCabs");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_cabAsFirstId",
                table: "serviceRequsets");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_cabAsFourthId",
                table: "serviceRequsets");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_cabAsSecondId",
                table: "serviceRequsets");

            migrationBuilder.DropIndex(
                name: "IX_serviceRequsets_cabAsThirdId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "cabAsFirstId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "cabAsFourthId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "cabAsSecondId",
                table: "serviceRequsets");

            migrationBuilder.DropColumn(
                name: "cabAsThirdId",
                table: "serviceRequsets");
        }
    }
}
