using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleNameEngToExistingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleNameEng",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "CompanyPermissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { 3, "CompanyRoleSeeEntireList" },
                    { 4, "CompanyUserSeeEntireList" },
                    { 5, "CompanyRoleCreator" }
                });

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RoleName", "RoleNameEng" },
                values: new object[] { "Twórca firmy", "Company creator" });

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RoleName", "RoleNameEng" },
                values: new object[] { "Dział oprogramowania", "Software department" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRoles_RoleName_RoleNameEng",
                table: "CompanyRoles",
                columns: new[] { "RoleName", "RoleNameEng" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyRoles_RoleName_RoleNameEng",
                table: "CompanyRoles");

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "RoleNameEng",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RoleName", "RoleNameEng" },
                values: new object[] { "Company creator", "" });

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RoleName", "RoleNameEng" },
                values: new object[] { "Software department", "" });
        }
    }
}
