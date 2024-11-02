using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.CompanyAPI.Migrations
{
	/// <inheritdoc />
	public partial class UpdateCompanyPermissionsAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "CompanyRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "PermissionName",
                value: "CompanyManager");

            migrationBuilder.UpdateData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "PermissionName",
                value: "Employee");

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsMain", "IsVisible", "RoleName" },
                values: new object[] { false, false, "Company creator" });

            migrationBuilder.InsertData(
                table: "CompanyRoles",
                columns: new[] { "Id", "IsMain", "IsVisible", "RoleName" },
                values: new object[] { 2, true, true, "Software department" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "CompanyRoles");

            migrationBuilder.UpdateData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "PermissionName",
                value: "Creator");

            migrationBuilder.UpdateData(
                table: "CompanyPermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "PermissionName",
                value: "Authorized to manage");

            migrationBuilder.InsertData(
                table: "CompanyPermissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[] { 3, "Employee" });

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsMain", "RoleName" },
                values: new object[] { true, "Software department" });
        }
    }
}
