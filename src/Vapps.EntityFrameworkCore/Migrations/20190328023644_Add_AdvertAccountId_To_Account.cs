using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Add_AdvertAccountId_To_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AdvertAccountId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "AdvertAccounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertAccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "AdvertAccounts");
        }
    }
}
