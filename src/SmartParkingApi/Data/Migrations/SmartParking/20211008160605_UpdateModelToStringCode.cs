using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class UpdateModelToStringCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardStatuses_CardStatusId",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardStatuses",
                table: "CardStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardStatusId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CardStatuses");

            migrationBuilder.DropColumn(
                name: "CardStatusId",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CardStatuses",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cards",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityCode",
                table: "Cards",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                table: "Cards",
                type: "nvarchar(15)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardStatuses",
                table: "CardStatuses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_StatusCode",
                table: "Cards",
                column: "StatusCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardStatuses_StatusCode",
                table: "Cards",
                column: "StatusCode",
                principalTable: "CardStatuses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardStatuses_StatusCode",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardStatuses",
                table: "CardStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Cards_StatusCode",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CardStatuses",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CardStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityCode",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CardStatusId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardStatuses",
                table: "CardStatuses",
                column: "Id");

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
        }
    }
}
