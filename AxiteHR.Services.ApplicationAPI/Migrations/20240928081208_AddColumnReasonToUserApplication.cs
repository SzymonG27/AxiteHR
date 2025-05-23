﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.ApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnReasonToUserApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "UserApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "UserApplications");
        }
    }
}
