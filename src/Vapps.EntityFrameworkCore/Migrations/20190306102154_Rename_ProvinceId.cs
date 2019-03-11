using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Rename_ProvinceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingProvice",
                table: "Orders",
                newName: "ShippingProvince");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingProvince",
                table: "Orders",
                newName: "ShippingProvice");
        }
    }
}
