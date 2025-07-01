using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.InvoiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceNumeration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Invoices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InvoiceSequence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CompanyUserId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    CurrentNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSequence", x => x.Id);
                    table.CheckConstraint("CK_Type_Enum1", "[Type] IN (1, 2)");
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Type_Enum",
                table: "Invoices",
                sql: "[Type] IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSequence_CompanyUserId_Type_Year_Month",
                table: "InvoiceSequence",
                columns: new[] { "CompanyUserId", "Type", "Year", "Month" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceSequence");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Type_Enum",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Invoices");
        }
    }
}
