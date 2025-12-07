using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class CompanyPermissionGroupInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissions_CompanyPermissionId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId",
                table: "CompanyUserPermissions");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyPermissionId",
                table: "CompanyUserPermissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyPermissionGroupId",
                table: "CompanyUserPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyPermissionGroup",
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
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_CompanyUserPermissions_CompanyPermissionGroupId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId_CompanyPermissionGroupId",
                table: "CompanyUserPermissions",
                columns: new[] { "CompanyUserId", "CompanyPermissionGroupId" },
                unique: true,
                filter: "CompanyPermissionGroupId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId_CompanyPermissionId",
                table: "CompanyUserPermissions",
                columns: new[] { "CompanyUserId", "CompanyPermissionId" },
                unique: true,
                filter: "CompanyPermissionId IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CompanyUserPermission_OneRequired",
                table: "CompanyUserPermissions",
                sql: "(CompanyPermissionId IS NOT NULL AND CompanyPermissionGroupId IS NULL) OR (CompanyPermissionId IS NULL AND CompanyPermissionGroupId IS NOT NULL)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissions_CompanyPermissionId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionId",
                principalTable: "CompanyPermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissionGroup_CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissions_CompanyPermissionId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroupPermission");

            migrationBuilder.DropTable(
                name: "CompanyPermissionGroup");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUserPermissions_CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId_CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId_CompanyPermissionId",
                table: "CompanyUserPermissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CompanyUserPermission_OneRequired",
                table: "CompanyUserPermissions");

            migrationBuilder.DropColumn(
                name: "CompanyPermissionGroupId",
                table: "CompanyUserPermissions");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyPermissionId",
                table: "CompanyUserPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId",
                table: "CompanyUserPermissions",
                column: "CompanyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserPermissions_CompanyPermissions_CompanyPermissionId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionId",
                principalTable: "CompanyPermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
