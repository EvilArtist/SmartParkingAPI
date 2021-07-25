using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddPriceBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceListConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    condition_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceListConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Calcutation_Type = table.Column<int>(type: "int", nullable: true),
                    Calcutation_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Calcutation_Price = table.Column<double>(type: "float", nullable: true),
                    Calcutation_HourBlock = table.Column<double>(type: "float", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_PriceListConditions_PriceListConditionId",
                        column: x => x.PriceListConditionId,
                        principalTable: "PriceListConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceLists_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceLists_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_PriceListConditionId",
                table: "PriceLists",
                column: "PriceListConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_SubscriptionTypeId",
                table: "PriceLists",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_VehicleTypeId",
                table: "PriceLists",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "PriceListConditions");
        }
    }
}
