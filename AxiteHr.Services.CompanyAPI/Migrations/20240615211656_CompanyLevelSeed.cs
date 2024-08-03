using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxiteHr.Services.CompanyAPI.Migrations
{
	/// <inheritdoc />
	public partial class CompanyLevelSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompanyLevels",
                columns: new[] { "Id", "MaxNumberOfWorkers" },
                values: new object[,]
                {
                    { 1, 10 },
                    { 2, 25 },
                    { 3, 50 },
                    { 4, 100 },
                    { 5, 2147483647 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyLevels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompanyLevels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CompanyLevels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CompanyLevels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CompanyLevels",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
