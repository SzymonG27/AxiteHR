using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class CompanyRoleAddColumnRoleNameEng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleNameEng",
                table: "CompanyRoles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleNameEng",
                value: "");

            migrationBuilder.UpdateData(
                table: "CompanyRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleNameEng",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleNameEng",
                table: "CompanyRoles");
        }
    }
}
