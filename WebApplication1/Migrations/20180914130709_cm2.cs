using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication1.Migrations
{
    public partial class cm2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companyModell",
                columns: table => new
                {
                    companyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyOrgNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    compayPgNr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyModell", x => x.companyId);
                });

            migrationBuilder.CreateTable(
                name: "customerModel",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyId = table.Column<int>(type: "int", nullable: true),
                    eMail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerModel", x => x.customerId);
                    table.ForeignKey(
                        name: "FK_customerModel_companyModell_companyId",
                        column: x => x.companyId,
                        principalTable: "companyModell",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "suplierModel",
                columns: table => new
                {
                    suplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    companyModellcompanyId = table.Column<int>(type: "int", nullable: true),
                    currencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currencyFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    invoiceDateFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    purchOrderNoFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierBgNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierBgNrFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierOrcNrFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierOrgNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierOrgNrFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierPgNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suplierPgNrFormat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suplierModel", x => x.suplierId);
                    table.ForeignKey(
                        name: "FK_suplierModel_companyModell_companyModellcompanyId",
                        column: x => x.companyModellcompanyId,
                        principalTable: "companyModell",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customerModel_companyId",
                table: "customerModel",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_suplierModel_companyModellcompanyId",
                table: "suplierModel",
                column: "companyModellcompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerModel");

            migrationBuilder.DropTable(
                name: "suplierModel");

            migrationBuilder.DropTable(
                name: "companyModell");
        }
    }
}
