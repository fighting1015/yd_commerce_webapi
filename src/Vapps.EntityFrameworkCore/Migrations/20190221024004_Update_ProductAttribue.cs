using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_ProductAttribue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotifyAdminForQuantityBelow",
                table: "Products",
                newName: "NotifyQuantityBelow");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductAttributeCombinations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductId",
                table: "ProductAttributeValues",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeValues_ProductId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductAttributeCombinations");

            migrationBuilder.RenameColumn(
                name: "NotifyQuantityBelow",
                table: "Products",
                newName: "NotifyAdminForQuantityBelow");
        }
    }
}
