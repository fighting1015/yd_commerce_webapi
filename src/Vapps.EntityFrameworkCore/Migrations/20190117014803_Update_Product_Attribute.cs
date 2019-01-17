using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_Product_Attribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductAttributeMappings",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "ProductAttributeId",
                table: "ProductAttributeMappings",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductAttributeCombinations",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMappings_ProductId",
                table: "ProductAttributeMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeCombinations_ProductId",
                table: "ProductAttributeCombinations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeCombinations_Products_ProductId",
                table: "ProductAttributeCombinations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeMappings_Products_ProductId",
                table: "ProductAttributeMappings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeCombinations_Products_ProductId",
                table: "ProductAttributeCombinations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeMappings_Products_ProductId",
                table: "ProductAttributeMappings");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeMappings_ProductId",
                table: "ProductAttributeMappings");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeCombinations_ProductId",
                table: "ProductAttributeCombinations");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductAttributeMappings",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "ProductAttributeId",
                table: "ProductAttributeMappings",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductAttributeCombinations",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
