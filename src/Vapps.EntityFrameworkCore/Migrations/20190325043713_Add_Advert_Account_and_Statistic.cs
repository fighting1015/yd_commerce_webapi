using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapps.Migrations
{
    public partial class Add_Advert_Account_and_Statistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginTime",
                table: "UserAccounts");

            migrationBuilder.AddColumn<string>(
                name: "ReturnValue",
                table: "AuditLogs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdvertAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ThirdpartyId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    DataAutoSync = table.Column<bool>(nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false),
                    TotalOrder = table.Column<decimal>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    AccessTokenExpiresIn = table.Column<DateTime>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiresIn = table.Column<DateTime>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvertDailyStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    AdvertAccountId = table.Column<long>(nullable: false),
                    DisplayNum = table.Column<int>(nullable: false),
                    ClickNum = table.Column<int>(nullable: false),
                    ClickPrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    ThDisplayCost = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false),
                    StatisticOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertDailyStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertDailyStatistics_AdvertAccounts_AdvertAccountId",
                        column: x => x.AdvertAccountId,
                        principalTable: "AdvertAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertDailyStatisticItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    AdvertDailyStatisticId = table.Column<long>(nullable: false),
                    HourOfDay = table.Column<int>(nullable: false),
                    DisplayNum = table.Column<int>(nullable: false),
                    ClickNum = table.Column<int>(nullable: false),
                    ClickPrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    ThDisplayCost = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AdvertStatisticsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertDailyStatisticItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertDailyStatisticItems_AdvertDailyStatistics_AdvertDailyStatisticId",
                        column: x => x.AdvertDailyStatisticId,
                        principalTable: "AdvertDailyStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertAccounts_TenantId_IsDeleted",
                table: "AdvertAccounts",
                columns: new[] { "TenantId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertDailyStatisticItems_AdvertDailyStatisticId",
                table: "AdvertDailyStatisticItems",
                column: "AdvertDailyStatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertDailyStatisticItems_TenantId_AdvertDailyStatisticId_IsDeleted",
                table: "AdvertDailyStatisticItems",
                columns: new[] { "TenantId", "AdvertDailyStatisticId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertDailyStatistics_AdvertAccountId",
                table: "AdvertDailyStatistics",
                column: "AdvertAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertDailyStatistics_TenantId_AdvertAccountId_IsDeleted",
                table: "AdvertDailyStatistics",
                columns: new[] { "TenantId", "AdvertAccountId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "OrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_TenantId_RoleId",
                table: "OrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertDailyStatisticItems");

            migrationBuilder.DropTable(
                name: "OrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AdvertDailyStatistics");

            migrationBuilder.DropTable(
                name: "AdvertAccounts");

            migrationBuilder.DropColumn(
                name: "ReturnValue",
                table: "AuditLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginTime",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginTime",
                table: "UserAccounts",
                nullable: true);
        }
    }
}
