using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class CompanyRoleCompanyIntermediateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserRoles_CompanyRoles_CompanyRoleId",
                table: "CompanyUserRoles");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "CompanyRoles");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "CompanyRoles");

            migrationBuilder.RenameColumn(
                name: "CompanyRoleId",
                table: "CompanyUserRoles",
                newName: "CompanyRoleCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUserRoles_CompanyRoleId",
                table: "CompanyUserRoles",
                newName: "IX_CompanyUserRoles_CompanyRoleCompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "CompanyPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CompanyRoleCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyRoleId = table.Column<int>(type: "int", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRoleCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRoleCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyRoleCompanies_CompanyRoles_CompanyRoleId",
                        column: x => x.CompanyRoleId,
                        principalTable: "CompanyRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRoleCompanies_CompanyId",
                table: "CompanyRoleCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRoleCompanies_CompanyRoleId_CompanyId",
                table: "CompanyRoleCompanies",
                columns: new[] { "CompanyRoleId", "CompanyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserRoles_CompanyRoleCompanies_CompanyRoleCompanyId",
                table: "CompanyUserRoles",
                column: "CompanyRoleCompanyId",
                principalTable: "CompanyRoleCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUserRoles_CompanyRoleCompanies_CompanyRoleCompanyId",
                table: "CompanyUserRoles");

            migrationBuilder.DropTable(
                name: "CompanyRoleCompanies");

            migrationBuilder.RenameColumn(
                name: "CompanyRoleCompanyId",
                table: "CompanyUserRoles",
                newName: "CompanyRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUserRoles_CompanyRoleCompanyId",
                table: "CompanyUserRoles",
                newName: "IX_CompanyUserRoles_CompanyRoleId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "CompanyRoles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "CompanyRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "CompanyRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "CompanyPermissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsMain", "IsVisible" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsMain", "IsVisible" },
                values: new object[] { true, true });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUserRoles_CompanyRoles_CompanyRoleId",
                table: "CompanyUserRoles",
                column: "CompanyRoleId",
                principalTable: "CompanyRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
