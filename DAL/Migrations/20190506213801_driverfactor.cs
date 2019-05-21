using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class driverfactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driverFactors",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    From = table.Column<DateTime>(nullable: true),
                    To = table.Column<DateTime>(nullable: true),
                    SeassionCount = table.Column<int>(nullable: false),
                    taxiCabeid = table.Column<int>(nullable: false),
                    serviceRequsetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driverFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driverFactors_serviceRequsets_serviceRequsetId",
                        column: x => x.serviceRequsetId,
                        principalTable: "serviceRequsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_driverFactors_taxiCabs_taxiCabeid",
                        column: x => x.taxiCabeid,
                        principalTable: "taxiCabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_driverFactors_serviceRequsetId",
                table: "driverFactors",
                column: "serviceRequsetId");

            migrationBuilder.CreateIndex(
                name: "IX_driverFactors_taxiCabeid",
                table: "driverFactors",
                column: "taxiCabeid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driverFactors");
        }
    }
}
