using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.InvoiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BlobFileName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientNip = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ClientStreet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientHouseNumber = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ClientPostalCode = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false),
                    ClientCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "varchar(26)", unicode: false, maxLength: 26, nullable: true),
                    PaymentDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    IsSplitPayment = table.Column<bool>(type: "bit", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.CheckConstraint("CK_BankAccountNumber_Format", "[BankAccountNumber] IS NULL OR (LEN([BankAccountNumber]) = 26 AND [BankAccountNumber] NOT LIKE '%[^0-9]%')");
                    table.CheckConstraint("CK_BankAccountNumber_RequiredIfPaymentByTransfer", "([PaymentMethod] <> 2) OR ([BankAccountNumber] IS NOT NULL AND LTRIM(RTRIM([BankAccountNumber])) <> '')");
                    table.CheckConstraint("CK_ClientNip_NIP_Format", "LEN([ClientNip]) = 10 AND [ClientNip] NOT LIKE '%[^0-9]%'");
                    table.CheckConstraint("CK_ClientPostalCode_PostalCode_Format", "[ClientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'");
                    table.CheckConstraint("CK_Currency_Enum", "[Currency] IN (1, 2)");
                    table.CheckConstraint("CK_PaymentMethod_Enum", "[PaymentMethod] IN (1, 2, 3)");
                    table.CheckConstraint("CK_Status_Enum", "[Status] IN (1, 2, 3)");
                });

            migrationBuilder.CreateTable(
                name: "InvoicePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<int>(type: "int", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePositions", x => x.Id);
                    table.CheckConstraint("CK_Unit_Enum", "[Unit] IN (1, 2)");
                    table.CheckConstraint("CK_VatRate_IntRange", "[VatRate] >= 0 AND [VatRate] <= 100");
                    table.ForeignKey(
                        name: "FK_InvoicePositions_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePositions_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicePositions");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
