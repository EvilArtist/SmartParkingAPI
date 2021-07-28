using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddCardAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardParkingAssignments",
                columns: table => new
                {
                    ParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardParkingAssignments", x => new { x.ParkingId, x.CardId });
                    table.ForeignKey(
                        name: "FK_CardParkingAssignments_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardParkingAssignments_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardParkingAssignments_CardId",
                table: "CardParkingAssignments",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardParkingAssignments");
        }
    }
}
