using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    public partial class UpdateCalculationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Calcutation_Type",
                table: "PriceLists",
                newName: "Calculation_Type");

            migrationBuilder.RenameColumn(
                name: "Calcutation_Price",
                table: "PriceLists",
                newName: "Calculation_Price");

            migrationBuilder.RenameColumn(
                name: "Calcutation_Name",
                table: "PriceLists",
                newName: "Calculation_Name");

            migrationBuilder.RenameColumn(
                name: "Calcutation_HourBlock",
                table: "PriceLists",
                newName: "Calculation_HourBlock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Calculation_Type",
                table: "PriceLists",
                newName: "Calcutation_Type");

            migrationBuilder.RenameColumn(
                name: "Calculation_Price",
                table: "PriceLists",
                newName: "Calcutation_Price");

            migrationBuilder.RenameColumn(
                name: "Calculation_Name",
                table: "PriceLists",
                newName: "Calcutation_Name");

            migrationBuilder.RenameColumn(
                name: "Calculation_HourBlock",
                table: "PriceLists",
                newName: "Calcutation_HourBlock");
        }
    }
}
