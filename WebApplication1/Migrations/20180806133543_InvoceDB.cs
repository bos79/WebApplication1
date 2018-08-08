using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class InvoceDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuxParam",
                columns: table => new
                {
                    AuxParmID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Company = table.Column<string>(nullable: true),
                    Verno = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuxParam", x => x.AuxParmID);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "eInvoice",
                columns: table => new
                {
                    eInvoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrencyCode = table.Column<string>(nullable: true),
                    PurchOrderNo = table.Column<string>(nullable: true),
                    PayType = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    InvoiceD = table.Column<string>(nullable: true),
                    InvoiceDateSpecified = table.Column<bool>(nullable: false),
                    DueD = table.Column<string>(nullable: true),
                    DueDateSpecified = table.Column<bool>(nullable: false),
                    VatAmount = table.Column<double>(nullable: false),
                    Authorizor = table.Column<string>(nullable: true),
                    OurCustomerNo = table.Column<string>(nullable: true),
                    InvoiceType = table.Column<string>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    SupplierNo = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true),
                    Ocr = table.Column<string>(nullable: true),
                    Freight = table.Column<double>(nullable: false),
                    FreightSpecified = table.Column<bool>(nullable: false),
                    VatCode = table.Column<string>(nullable: true),
                    Pg = table.Column<string>(nullable: true),
                    Bg = table.Column<string>(nullable: true),
                    OrgNo = table.Column<string>(nullable: true),
                    OurReference = table.Column<string>(nullable: true),
                    YourReference = table.Column<string>(nullable: true),
                    InvoiceRecipient = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Vat = table.Column<double>(nullable: false),
                    InvoiceFee = table.Column<double>(nullable: false),
                    AuxParmID = table.Column<int>(nullable: true),
                    Project = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eInvoice", x => x.eInvoiceId);
                    table.ForeignKey(
                        name: "FK_eInvoice_AuxParam_AuxParmID",
                        column: x => x.AuxParmID,
                        principalTable: "AuxParam",
                        principalColumn: "AuxParmID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAccounts",
                columns: table => new
                {
                    InvoiceAccountsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(nullable: true),
                    Profit_centre = table.Column<string>(nullable: true),
                    Project = table.Column<string>(nullable: true),
                    AccAuthorizor = table.Column<string>(nullable: true),
                    DebetAmount = table.Column<double>(nullable: false),
                    CreditAmount = table.Column<double>(nullable: false),
                    Reg_date = table.Column<DateTime>(nullable: false),
                    eInvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAccounts", x => x.InvoiceAccountsId);
                    table.ForeignKey(
                        name: "FK_InvoiceAccounts_eInvoice_eInvoiceId",
                        column: x => x.eInvoiceId,
                        principalTable: "eInvoice",
                        principalColumn: "eInvoiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceRows",
                columns: table => new
                {
                    InvoiceRowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemRow = table.Column<string>(nullable: true),
                    eInvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRows", x => x.InvoiceRowId);
                    table.ForeignKey(
                        name: "FK_InvoiceRows_eInvoice_eInvoiceId",
                        column: x => x.eInvoiceId,
                        principalTable: "eInvoice",
                        principalColumn: "eInvoiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eInvoice_AuxParmID",
                table: "eInvoice",
                column: "AuxParmID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAccounts_eInvoiceId",
                table: "InvoiceAccounts",
                column: "eInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRows_eInvoiceId",
                table: "InvoiceRows",
                column: "eInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "InvoiceAccounts");

            migrationBuilder.DropTable(
                name: "InvoiceRows");

            migrationBuilder.DropTable(
                name: "eInvoice");

            migrationBuilder.DropTable(
                name: "AuxParam");
        }
    }
}
