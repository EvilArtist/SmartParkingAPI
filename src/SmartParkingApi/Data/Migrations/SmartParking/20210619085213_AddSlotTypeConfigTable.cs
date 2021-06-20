using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddSlotTypeConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkingId",
                table: "SerialPortConfigurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingId",
                table: "CameraConfigurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParkingConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SlotTypeConfigurations",
                columns: table => new
                {
                    ParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlotTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SlotCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotTypeConfigurations", x => new { x.ParkingId, x.SlotTypeId });
                    table.ForeignKey(
                        name: "FK_SlotTypeConfigurations_ParkingConfig_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "ParkingConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlotTypeConfigurations_SlotTypes_SlotTypeId",
                        column: x => x.SlotTypeId,
                        principalTable: "SlotTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerialPortConfigurations_ParkingId",
                table: "SerialPortConfigurations",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraConfigurations_ParkingId",
                table: "CameraConfigurations",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotTypeConfigurations_SlotTypeId",
                table: "SlotTypeConfigurations",
                column: "SlotTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CameraConfigurations_ParkingConfig_ParkingId",
                table: "CameraConfigurations",
                column: "ParkingId",
                principalTable: "ParkingConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialPortConfigurations_ParkingConfig_ParkingId",
                table: "SerialPortConfigurations",
                column: "ParkingId",
                principalTable: "ParkingConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CameraConfigurations_ParkingConfig_ParkingId",
                table: "CameraConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialPortConfigurations_ParkingConfig_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropTable(
                name: "SlotTypeConfigurations");

            migrationBuilder.DropTable(
                name: "ParkingConfig");

            migrationBuilder.DropIndex(
                name: "IX_SerialPortConfigurations_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_CameraConfigurations_ParkingId",
                table: "CameraConfigurations");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "CameraConfigurations");
        }
    }
}
