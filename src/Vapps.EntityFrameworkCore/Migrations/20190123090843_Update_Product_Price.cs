using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_Product_Price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCost",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<decimal>(
                name: "GoodCost",
                table: "Products",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverriddenPrice",
                table: "ProductAttributeCombinations",
                type: "decimal(18, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverriddenGoodCost",
                table: "ProductAttributeCombinations",
                type: "decimal(18, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeCombinations_TenantId_ProductId",
                table: "ProductAttributeCombinations",
                columns: new[] { "TenantId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeCombinations_TenantId_ProductId",
                table: "ProductAttributeCombinations");

            migrationBuilder.DropColumn(
                name: "GoodCost",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AddColumn<decimal>(
                name: "ProductCost",
                table: "Products",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverriddenPrice",
                table: "ProductAttributeCombinations",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverriddenGoodCost",
                table: "ProductAttributeCombinations",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)",
                oldNullable: true);
        }
    }
}
