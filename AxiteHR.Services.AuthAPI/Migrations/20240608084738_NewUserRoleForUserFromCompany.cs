using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.AuthAPI.Migrations
{
	/// <inheritdoc />
	public partial class NewUserRoleForUserFromCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "87ec7457-f89e-4c4b-940d-31ec35e51e3f", null, "UserFromCompany", "USERFROMCOMPANY" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87ec7457-f89e-4c4b-940d-31ec35e51e3f");
        }
    }
}
