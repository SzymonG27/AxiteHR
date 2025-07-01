using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.InvoiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class RecipientOnInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientCity",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientHouseNumber",
                table: "Invoices",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientNip",
                table: "Invoices",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientPostalCode",
                table: "Invoices",
                type: "varchar(6)",
                unicode: false,
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientStreet",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RecipientNip_NIP_Format",
                table: "Invoices",
                sql: "LEN([RecipientNip]) = 10 AND [RecipientNip] NOT LIKE '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RecipientPostalCode_PostalCode_Format",
                table: "Invoices",
                sql: "[RecipientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_RecipientNip_NIP_Format",
                table: "Invoices");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RecipientPostalCode_PostalCode_Format",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientCity",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientHouseNumber",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientNip",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientPostalCode",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecipientStreet",
                table: "Invoices");
        }
    }
}
