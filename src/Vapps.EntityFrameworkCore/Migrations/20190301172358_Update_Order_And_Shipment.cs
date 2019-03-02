using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_Order_And_Shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Orders_OrderId1",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_OrderId1",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Logisticses");

            migrationBuilder.RenameColumn(
                name: "TrackingNumber",
                table: "Shipments",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "DeliveryDateUtc",
                table: "Shipments",
                newName: "ReceivedOn");

            migrationBuilder.RenameColumn(
                name: "ShippingTepephone",
                table: "Orders",
                newName: "ShippingPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PaidDateUtc",
                table: "Orders",
                newName: "ReceivedOn");

            migrationBuilder.RenameColumn(
                name: "CustomerIp",
                table: "Orders",
                newName: "IpAddress");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "Shipments",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                table: "Shipments",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "LogisticsNumber",
                table: "Shipments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Shipments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVolume",
                table: "Shipments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidOn",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Orders_OrderId",
                table: "Shipments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Orders_OrderId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "LogisticsNumber",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "TotalVolume",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaidOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ReceivedOn",
                table: "Shipments",
                newName: "DeliveryDateUtc");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "Shipments",
                newName: "TrackingNumber");

            migrationBuilder.RenameColumn(
                name: "ShippingPhoneNumber",
                table: "Orders",
                newName: "ShippingTepephone");

            migrationBuilder.RenameColumn(
                name: "ReceivedOn",
                table: "Orders",
                newName: "PaidDateUtc");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "Orders",
                newName: "CustomerIp");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "Shipments",
                type: "decimal(18, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Shipments",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "OrderId1",
                table: "Shipments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Logisticses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId1",
                table: "Shipments",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Orders_OrderId1",
                table: "Shipments",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
