using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Data.Migrations.IdentityServer.ApplicationDb
{
    public partial class AddClientIdforRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerRole",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsCustomerRole",
                table: "AspNetRoles");
        }
    }
}
