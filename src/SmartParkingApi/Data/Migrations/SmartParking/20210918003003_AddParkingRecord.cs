using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddParkingRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingRecordStatuses",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingRecordStatuses", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ParkingRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckinParkingLaneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckoutParkingLaneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckinEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckoutEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    URLCheckinFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URLCheckinBackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URLCheckoutFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URLCheckoutBackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckinPlateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutPlateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_ParkingLanes_CheckinParkingLaneId",
                        column: x => x.CheckinParkingLaneId,
                        principalTable: "ParkingLanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_ParkingLanes_CheckoutParkingLaneId",
                        column: x => x.CheckoutParkingLaneId,
                        principalTable: "ParkingLanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_ParkingRecordStatuses_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "ParkingRecordStatuses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    HourBlock = table.Column<double>(type: "float", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceItem_ParkingRecords_ParkingRecordId",
                        column: x => x.ParkingRecordId,
                        principalTable: "ParkingRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_CardId",
                table: "ParkingRecords",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_CheckinParkingLaneId",
                table: "ParkingRecords",
                column: "CheckinParkingLaneId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_CheckoutParkingLaneId",
                table: "ParkingRecords",
                column: "CheckoutParkingLaneId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_StatusCode",
                table: "ParkingRecords",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_SubscriptionTypeId",
                table: "ParkingRecords",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_VehicleTypeId",
                table: "ParkingRecords",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceItem_ParkingRecordId",
                table: "PriceItem",
                column: "ParkingRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceItem");

            migrationBuilder.DropTable(
                name: "ParkingRecords");

            migrationBuilder.DropTable(
                name: "ParkingRecordStatuses");
        }
    }
}
