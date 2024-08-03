using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxiteHr.Services.CompanyAPI.Migrations
{
	/// <inheritdoc />
	public partial class InitialCompanyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxNumberOfWorkers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLevelId = table.Column<int>(type: "int", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_CompanyLevels_CompanyLevelId",
                        column: x => x.CompanyLevelId,
                        principalTable: "CompanyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyUserId = table.Column<int>(type: "int", nullable: false),
                    CompanyPermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUserPermissions_CompanyPermissions_CompanyPermissionId",
                        column: x => x.CompanyPermissionId,
                        principalTable: "CompanyPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyUserPermissions_CompanyUsers_CompanyUserId",
                        column: x => x.CompanyUserId,
                        principalTable: "CompanyUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyUserId = table.Column<int>(type: "int", nullable: false),
                    CompanyRoleId = table.Column<int>(type: "int", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUserRoles_CompanyRoles_CompanyRoleId",
                        column: x => x.CompanyRoleId,
                        principalTable: "CompanyRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyUserRoles_CompanyUsers_CompanyUserId",
                        column: x => x.CompanyUserId,
                        principalTable: "CompanyUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CompanyPermissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { 1, "Creator" },
                    { 2, "Authorized to manage" },
                    { 3, "Employee" }
                });

            migrationBuilder.InsertData(
                table: "CompanyRoles",
                columns: new[] { "Id", "IsMain", "RoleName" },
                values: new object[] { 1, true, "Software department" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyLevelId",
                table: "Companies",
                column: "CompanyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPermissions_CompanyPermissionId",
                table: "CompanyUserPermissions",
                column: "CompanyPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPermissions_CompanyUserId",
                table: "CompanyUserPermissions",
                column: "CompanyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserRoles_CompanyRoleId",
                table: "CompanyUserRoles",
                column: "CompanyRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserRoles_CompanyUserId",
                table: "CompanyUserRoles",
                column: "CompanyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId",
                table: "CompanyUsers",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUserPermissions");

            migrationBuilder.DropTable(
                name: "CompanyUserRoles");

            migrationBuilder.DropTable(
                name: "CompanyPermissions");

            migrationBuilder.DropTable(
                name: "CompanyRoles");

            migrationBuilder.DropTable(
                name: "CompanyUsers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyLevels");
        }
    }
}
