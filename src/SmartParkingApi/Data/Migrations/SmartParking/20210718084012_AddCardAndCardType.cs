using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class AddCardAndCardType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SubscriptionTypes_SubscriptionTypeId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_VehicleTypes_VehicleTypeId",
                table: "Cards");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleTypeId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionTypeId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CardStatusId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentityCode",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardStatusId",
                table: "Cards",
                column: "CardStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardStatuses_CardStatusId",
                table: "Cards",
                column: "CardStatusId",
                principalTable: "CardStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SubscriptionTypes_SubscriptionTypeId",
                table: "Cards",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_VehicleTypes_VehicleTypeId",
                table: "Cards",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardStatuses_CardStatusId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SubscriptionTypes_SubscriptionTypeId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_VehicleTypes_VehicleTypeId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CardStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardStatusId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardStatusId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IdentityCode",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleTypeId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionTypeId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SubscriptionTypes_SubscriptionTypeId",
                table: "Cards",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_VehicleTypes_VehicleTypeId",
                table: "Cards",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
