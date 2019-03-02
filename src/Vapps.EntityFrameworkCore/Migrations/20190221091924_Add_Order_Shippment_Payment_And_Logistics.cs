using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Add_Order_Shippment_Payment_And_Logistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodsUrl",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Logisticses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logisticses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    OrderId = table.Column<long>(nullable: false),
                    ExternalOrderNumber = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    PaymentBankSystemName = table.Column<string>(nullable: true),
                    Payer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OrderNumber = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ShippingProvice = table.Column<string>(nullable: true),
                    ShippingCity = table.Column<string>(nullable: true),
                    ShippingDistrict = table.Column<string>(nullable: true),
                    ShippingAddress = table.Column<string>(nullable: true),
                    ShippingTepephone = table.Column<string>(nullable: true),
                    ShippingName = table.Column<string>(nullable: true),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RewardAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    PaymentMethodAdditionalFee = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    SubTotalDiscountAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RefundedAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    CustomerIp = table.Column<string>(nullable: true),
                    PaidDateUtc = table.Column<DateTime>(nullable: true),
                    ShippingMethod = table.Column<string>(nullable: true),
                    AdminComment = table.Column<string>(nullable: true),
                    CustomerComment = table.Column<string>(nullable: true),
                    OrderStatus = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    ShippingStatus = table.Column<int>(nullable: false),
                    OrderSource = table.Column<int>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantLogisticses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    LogisticsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantLogisticses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    OrderItemNumber = table.Column<string>(nullable: true),
                    OrderId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    DiscountAmount = table.Column<decimal>(nullable: false),
                    OriginalProductCost = table.Column<decimal>(nullable: false),
                    AttributeDescription = table.Column<string>(nullable: true),
                    AttributesJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    TrackingNumber = table.Column<string>(nullable: true),
                    TotalWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: true),
                    DeliveryDateUtc = table.Column<DateTime>(nullable: true),
                    AdminComment = table.Column<string>(nullable: true),
                    LogisticsId = table.Column<int>(nullable: true),
                    LogisticsName = table.Column<string>(nullable: true),
                    ShipmentDetail = table.Column<string>(nullable: true),
                    OrderId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ShipmentId = table.Column<long>(nullable: false),
                    OrderItemId = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentItems_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logisticses_IsDeleted",
                table: "Logisticses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_TenantId_OrderId_IsDeleted",
                table: "OrderItems",
                columns: new[] { "TenantId", "OrderId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_TenantId_ProductId_IsDeleted",
                table: "OrderItems",
                columns: new[] { "TenantId", "ProductId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_TenantId_OrderId_IsDeleted",
                table: "OrderPayments",
                columns: new[] { "TenantId", "OrderId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId_OrderStatus",
                table: "Orders",
                columns: new[] { "TenantId", "OrderStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId_PaymentStatus",
                table: "Orders",
                columns: new[] { "TenantId", "PaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId_ShippingStatus",
                table: "Orders",
                columns: new[] { "TenantId", "ShippingStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId_UserId_IsDeleted",
                table: "Orders",
                columns: new[] { "TenantId", "UserId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItems_ShipmentId",
                table: "ShipmentItems",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItems_TenantId_IsDeleted",
                table: "ShipmentItems",
                columns: new[] { "TenantId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId1",
                table: "Shipments",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_TenantId_IsDeleted",
                table: "Shipments",
                columns: new[] { "TenantId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantLogisticses_TenantId",
                table: "TenantLogisticses",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logisticses");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderPayments");

            migrationBuilder.DropTable(
                name: "ShipmentItems");

            migrationBuilder.DropTable(
                name: "TenantLogisticses");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "GoodsUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Products");
        }
    }
}
