using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.ApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserIdToCompanyUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCompanyDaysOffs_UserId",
                table: "UserCompanyDaysOffs");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanyDaysOffs_UserId_CompanyId",
                table: "UserCompanyDaysOffs");

            migrationBuilder.DropIndex(
                name: "IX_UserApplications_UserId",
                table: "UserApplications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserCompanyDaysOffs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserApplications");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "UserCompanyDaysOffs",
                newName: "CompanyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCompanyDaysOffs_CompanyId",
                table: "UserCompanyDaysOffs",
                newName: "IX_UserCompanyDaysOffs_CompanyUserId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyUserId",
                table: "UserApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_CompanyUserId",
                table: "UserApplications",
                column: "CompanyUserId")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserApplications_CompanyUserId",
                table: "UserApplications");

            migrationBuilder.DropColumn(
                name: "CompanyUserId",
                table: "UserApplications");

            migrationBuilder.RenameColumn(
                name: "CompanyUserId",
                table: "UserCompanyDaysOffs",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCompanyDaysOffs_CompanyUserId",
                table: "UserCompanyDaysOffs",
                newName: "IX_UserCompanyDaysOffs_CompanyId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserCompanyDaysOffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyDaysOffs_UserId",
                table: "UserCompanyDaysOffs",
                column: "UserId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyDaysOffs_UserId_CompanyId",
                table: "UserCompanyDaysOffs",
                columns: new[] { "UserId", "CompanyId" },
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_UserId",
                table: "UserApplications",
                column: "UserId")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
