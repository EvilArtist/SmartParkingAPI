using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class RemoveParkingSerialPortRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SerialPortConfigurations_Parkings_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_SerialPortConfigurations_ParkingId",
                table: "SerialPortConfigurations");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "SerialPortConfigurations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkingId",
                table: "SerialPortConfigurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SerialPortConfigurations_ParkingId",
                table: "SerialPortConfigurations",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SerialPortConfigurations_Parkings_ParkingId",
                table: "SerialPortConfigurations",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
