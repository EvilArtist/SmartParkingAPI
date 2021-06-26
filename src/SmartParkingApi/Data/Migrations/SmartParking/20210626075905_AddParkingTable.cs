using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddParkingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CameraConfigurations_ParkingConfig_ParkingId",
                table: "CameraConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialPortConfigurations_ParkingConfig_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotTypeConfigurations_ParkingConfig_ParkingId",
                table: "SlotTypeConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingConfig",
                table: "ParkingConfig");

            migrationBuilder.RenameTable(
                name: "ParkingConfig",
                newName: "Parkings");

            migrationBuilder.RenameColumn(
                name: "ParkingId",
                table: "CameraConfigurations",
                newName: "ParkingLaneId");

            migrationBuilder.RenameIndex(
                name: "IX_CameraConfigurations_ParkingId",
                table: "CameraConfigurations",
                newName: "IX_CameraConfigurations_ParkingLaneId");

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingLaneId",
                table: "SerialPortConfigurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Parkings",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Parkings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parkings",
                table: "Parkings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ParkingLanes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLanes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingLanes_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerialPortConfigurations_ParkingLaneId",
                table: "SerialPortConfigurations",
                column: "ParkingLaneId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLanes_ParkingId",
                table: "ParkingLanes",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CameraConfigurations_ParkingLanes_ParkingLaneId",
                table: "CameraConfigurations",
                column: "ParkingLaneId",
                principalTable: "ParkingLanes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialPortConfigurations_ParkingLanes_ParkingLaneId",
                table: "SerialPortConfigurations",
                column: "ParkingLaneId",
                principalTable: "ParkingLanes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialPortConfigurations_Parkings_ParkingId",
                table: "SerialPortConfigurations",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SlotTypeConfigurations_Parkings_ParkingId",
                table: "SlotTypeConfigurations",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CameraConfigurations_ParkingLanes_ParkingLaneId",
                table: "CameraConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialPortConfigurations_ParkingLanes_ParkingLaneId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialPortConfigurations_Parkings_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotTypeConfigurations_Parkings_ParkingId",
                table: "SlotTypeConfigurations");

            migrationBuilder.DropTable(
                name: "ParkingLanes");

            migrationBuilder.DropIndex(
                name: "IX_SerialPortConfigurations_ParkingLaneId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parkings",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "ParkingLaneId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Parkings");

            migrationBuilder.RenameTable(
                name: "Parkings",
                newName: "ParkingConfig");

            migrationBuilder.RenameColumn(
                name: "ParkingLaneId",
                table: "CameraConfigurations",
                newName: "ParkingId");

            migrationBuilder.RenameIndex(
                name: "IX_CameraConfigurations_ParkingLaneId",
                table: "CameraConfigurations",
                newName: "IX_CameraConfigurations_ParkingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingConfig",
                table: "ParkingConfig",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SlotTypeConfigurations_ParkingConfig_ParkingId",
                table: "SlotTypeConfigurations",
                column: "ParkingId",
                principalTable: "ParkingConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
