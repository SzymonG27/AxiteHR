using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHr.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class IsSupervisorColumnForCompanyUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupervisor",
                table: "CompanyUserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupervisor",
                table: "CompanyUserRoles");
        }
    }
}
