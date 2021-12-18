using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class UpdatePriceBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_PriceListConditions_PriceListConditionId",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_SubscriptionTypes_SubscriptionTypeId",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_VehicleTypes_VehicleTypeId",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_PriceListConditionId",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_SubscriptionTypeId",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "Calculation_Name",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "PriceListConditionId",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "SubscriptionTypeId",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "PriceListConditions");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "PriceListConditions");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "PriceLists",
                newName: "PriceBookId");

            migrationBuilder.RenameColumn(
                name: "Calculation_Type",
                table: "PriceLists",
                newName: "Calculation_FormularType");

            migrationBuilder.RenameColumn(
                name: "Calculation_Price",
                table: "PriceLists",
                newName: "Calculation_PayPrice");

            migrationBuilder.RenameIndex(
                name: "IX_PriceLists_VehicleTypeId",
                table: "PriceLists",
                newName: "IX_PriceLists_PriceBookId");

            migrationBuilder.AddColumn<double>(
                name: "Calculation_SubscriptionPrice",
                table: "PriceLists",
                type: "float",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "PriceLists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Offset",
                table: "PriceLists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "PriceLists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "PriceBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceBookConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceBooks_PriceListConditions_PriceBookConditionId",
                        column: x => x.PriceBookConditionId,
                        principalTable: "PriceListConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceBooks_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceBooks_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceBooks_PriceBookConditionId",
                table: "PriceBooks",
                column: "PriceBookConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBooks_SubscriptionTypeId",
                table: "PriceBooks",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBooks_VehicleTypeId",
                table: "PriceBooks",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_PriceBooks_PriceBookId",
                table: "PriceLists",
                column: "PriceBookId",
                principalTable: "PriceBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_PriceBooks_PriceBookId",
                table: "PriceLists");

            migrationBuilder.DropTable(
                name: "PriceBooks");

            migrationBuilder.DropColumn(
                name: "Calculation_SubscriptionPrice",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "Offset",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "PriceLists");

            migrationBuilder.RenameColumn(
                name: "PriceBookId",
                table: "PriceLists",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "Calculation_PayPrice",
                table: "PriceLists",
                newName: "Calculation_Price");

            migrationBuilder.RenameColumn(
                name: "Calculation_FormularType",
                table: "PriceLists",
                newName: "Calculation_Type");

            migrationBuilder.RenameIndex(
                name: "IX_PriceLists_PriceBookId",
                table: "PriceLists",
                newName: "IX_PriceLists_VehicleTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Calculation_Name",
                table: "PriceLists",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PriceListConditionId",
                table: "PriceLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionTypeId",
                table: "PriceLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "PriceListConditions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "PriceListConditions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_PriceListConditionId",
                table: "PriceLists",
                column: "PriceListConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_SubscriptionTypeId",
                table: "PriceLists",
                column: "SubscriptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_PriceListConditions_PriceListConditionId",
                table: "PriceLists",
                column: "PriceListConditionId",
                principalTable: "PriceListConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_SubscriptionTypes_SubscriptionTypeId",
                table: "PriceLists",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_VehicleTypes_VehicleTypeId",
                table: "PriceLists",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
