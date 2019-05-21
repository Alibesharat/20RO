using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IamgePath",
                table: "serviceRequsets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Hs_Change = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ammount = table.Column<string>(nullable: true),
                    RequsetServiceId = table.Column<int>(nullable: true),
                    ParrentId = table.Column<int>(nullable: false),
                    TrackingCode = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_studentParents_ParrentId",
                        column: x => x.ParrentId,
                        principalTable: "studentParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_serviceRequsets_RequsetServiceId",
                        column: x => x.RequsetServiceId,
                        principalTable: "serviceRequsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_ParrentId",
                table: "payments",
                column: "ParrentId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_RequsetServiceId",
                table: "payments",
                column: "RequsetServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropColumn(
                name: "IamgePath",
                table: "serviceRequsets");
        }
    }
}
