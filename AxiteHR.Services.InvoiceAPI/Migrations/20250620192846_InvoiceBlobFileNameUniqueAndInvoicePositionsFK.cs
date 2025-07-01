using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxiteHR.Services.InvoiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceBlobFileNameUniqueAndInvoicePositionsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSequence",
                table: "InvoiceSequence");

            migrationBuilder.RenameTable(
                name: "InvoiceSequence",
                newName: "InvoiceSequences");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceSequence_CompanyUserId_Type_Year_Month",
                table: "InvoiceSequences",
                newName: "IX_InvoiceSequences_CompanyUserId_Type_Year_Month");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSequences",
                table: "InvoiceSequences",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BlobFileName",
                table: "Invoices",
                column: "BlobFileName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_BlobFileName",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSequences",
                table: "InvoiceSequences");

            migrationBuilder.RenameTable(
                name: "InvoiceSequences",
                newName: "InvoiceSequence");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceSequences_CompanyUserId_Type_Year_Month",
                table: "InvoiceSequence",
                newName: "IX_InvoiceSequence_CompanyUserId_Type_Year_Month");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSequence",
                table: "InvoiceSequence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
