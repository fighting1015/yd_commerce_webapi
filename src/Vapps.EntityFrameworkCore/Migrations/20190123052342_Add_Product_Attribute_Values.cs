using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Add_Product_Attribute_Values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProductAttributeMappingId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    PictureId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_ProductAttributeMappings_ProductAttri~",
                        column: x => x.ProductAttributeMappingId,
                        principalTable: "ProductAttributeMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductAttributeMappingId",
                table: "ProductAttributeValues",
                column: "ProductAttributeMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_TenantId_ProductId_ProductAttributeMa~",
                table: "ProductAttributeValues",
                columns: new[] { "TenantId", "ProductId", "ProductAttributeMappingId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeValues");
        }
    }
}
