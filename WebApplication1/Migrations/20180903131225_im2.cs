using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication1.Migrations
{
    public partial class im2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_eInvoice_eInvoiceId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "eInvoceId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "eInvoiceId",
                table: "Images",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageMimeType",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_eInvoice_eInvoiceId",
                table: "Images",
                column: "eInvoiceId",
                principalTable: "eInvoice",
                principalColumn: "eInvoiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_eInvoice_eInvoiceId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "imageMimeType",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "eInvoiceId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "eInvoceId",
                table: "Images",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_eInvoice_eInvoiceId",
                table: "Images",
                column: "eInvoiceId",
                principalTable: "eInvoice",
                principalColumn: "eInvoiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
