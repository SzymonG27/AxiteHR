using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxiteHR.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAndUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "775da6ba-b138-4551-aa18-fb161e8ffbc9", null, "User", "USER" },
                    { "c6b3a381-6ffb-4b88-8aa8-877421f520c7", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "775da6ba-b138-4551-aa18-fb161e8ffbc9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6b3a381-6ffb-4b88-8aa8-877421f520c7");
        }
    }
}
