using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Update_Attribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PredefinedProductAttributeValueId",
                table: "ProductAttributeValues",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductAttributes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ProductAttributeId",
                table: "PredefinedProductAttributeValues",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMappings_ProductAttributeId",
                table: "ProductAttributeMappings",
                column: "ProductAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeMappings_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeMappings",
                column: "ProductAttributeId",
                principalTable: "ProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeMappings_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeMappings");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeMappings_ProductAttributeId",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "PredefinedProductAttributeValueId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductAttributes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductAttributeId",
                table: "PredefinedProductAttributeValues",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
