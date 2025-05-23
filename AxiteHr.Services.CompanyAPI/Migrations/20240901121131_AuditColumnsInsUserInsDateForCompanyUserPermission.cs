﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.CompanyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AuditColumnsInsUserInsDateForCompanyUserPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsDate",
                table: "CompanyUserPermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "InsUserId",
                table: "CompanyUserPermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsDate",
                table: "CompanyUserPermissions");

            migrationBuilder.DropColumn(
                name: "InsUserId",
                table: "CompanyUserPermissions");
        }
    }
}
