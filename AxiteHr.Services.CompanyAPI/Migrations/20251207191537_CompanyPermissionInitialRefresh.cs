using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class CompanyPermissionInitialRefresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissionGroup_CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroupPermission");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroup");

            migrationBuilder.CreateTable(
                name: "CompanyPermissionGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermissionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPermissionGroupPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyPermissionGroupId = table.Column<int>(type: "int", nullable: false),
                    CompanyPermissionId = table.Column<int>(type: "int", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermissionGroupPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroupPermissions_CompanyPermissionGroups_CompanyPermissionGroupId",
                        column: x => x.CompanyPermissionGroupId,
                        principalTable: "CompanyPermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroupPermissions_CompanyPermissions_CompanyPermissionId",
                        column: x => x.CompanyPermissionId,
                        principalTable: "CompanyPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CompanyPermissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { 6, "CompanyPermissionManager" },
                    { 7, "CompanyRoleManager" },
                    { 8, "CompanyUserManager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroupPermissions_CompanyPermissionGroupId_CompanyPermissionId",
                table: "CompanyPermissionGroupPermissions",
                columns: new[] { "CompanyPermissionGroupId", "CompanyPermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroupPermissions_CompanyPermissionId",
                table: "CompanyPermissionGroupPermissions",
                column: "CompanyPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroups_CompanyId_Name",
                table: "CompanyPermissionGroups",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissionGroups_CompanyPermissionGroupId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionGroupId",
                principalTable: "CompanyPermissionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissionGroups_CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroupPermissions");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroups");

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.CreateTable(
                name: "CompanyPermissionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermissionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroup_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPermissionGroupPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyPermissionGroupId = table.Column<int>(type: "int", nullable: false),
                    CompanyPermissionId = table.Column<int>(type: "int", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermissionGroupPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroupPermission_CompanyPermissionGroup_CompanyPermissionGroupId",
                        column: x => x.CompanyPermissionGroupId,
                        principalTable: "CompanyPermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPermissionGroupPermission_CompanyPermissions_CompanyPermissionId",
                        column: x => x.CompanyPermissionId,
                        principalTable: "CompanyPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroup_CompanyId_Name",
                table: "CompanyPermissionGroup",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroupPermission_CompanyPermissionGroupId_CompanyPermissionId",
                table: "CompanyPermissionGroupPermission",
                columns: new[] { "CompanyPermissionGroupId", "CompanyPermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermissionGroupPermission_CompanyPermissionId",
                table: "CompanyPermissionGroupPermission",
                column: "CompanyPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissionGroup_CompanyPermissionGroupId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionGroupId",
                principalTable: "CompanyPermissionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
