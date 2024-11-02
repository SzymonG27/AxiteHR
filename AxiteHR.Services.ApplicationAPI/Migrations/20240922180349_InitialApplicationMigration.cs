using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.ApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialApplicationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyDaysOffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    DaysOff = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyDaysOffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserApplicationSupervisorAccepteds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserApplicationId = table.Column<int>(type: "int", nullable: false),
                    SupervisorAcceptedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplicationSupervisorAccepteds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserApplicationSupervisorAccepteds_UserApplications_UserApplicationId",
                        column: x => x.UserApplicationId,
                        principalTable: "UserApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_UserId",
                table: "UserApplications",
                column: "UserId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationSupervisorAccepteds_SupervisorAcceptedId",
                table: "UserApplicationSupervisorAccepteds",
                column: "SupervisorAcceptedId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationSupervisorAccepteds_UserApplicationId",
                table: "UserApplicationSupervisorAccepteds",
                column: "UserApplicationId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyDaysOffs_CompanyId",
                table: "UserCompanyDaysOffs",
                column: "CompanyId")
                .Annotation("SqlServer:Clustered", false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserApplicationSupervisorAccepteds");

            migrationBuilder.DropTable(
                name: "UserCompanyDaysOffs");

            migrationBuilder.DropTable(
                name: "UserApplications");
        }
    }
}
