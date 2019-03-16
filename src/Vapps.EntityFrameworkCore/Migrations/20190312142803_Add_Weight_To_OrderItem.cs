using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Add_Weight_To_OrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedOn",
                table: "Shipments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedOn",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "OrderItems");
        }
    }
}
