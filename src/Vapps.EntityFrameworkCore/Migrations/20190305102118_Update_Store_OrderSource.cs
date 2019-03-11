using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_Store_OrderSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderSourceType",
                table: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "OrderSource",
                table: "Stores",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderSource",
                table: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "OrderSourceType",
                table: "Stores",
                nullable: false,
                defaultValue: 0);
        }
    }
}
